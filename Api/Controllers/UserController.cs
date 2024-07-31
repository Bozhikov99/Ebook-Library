using Api.EmailService;
using Api.Extenstions;
using Api.Hypermedia;
using AutoMapper;
using Common;
using Common.ApiConstants;
using Common.MessageConstants;
using Core.Common.Interfaces;
using Core.Users.Commands.ConfirmEmail;
using Core.Users.Commands.EditUserRoles;
using Core.Users.Commands.Login;
using Core.Users.Commands.Register;
using Core.Users.Queries.GetAllUsers;
using Core.Users.Queries.GetProfile;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Api.Controllers
{
    [Route("[controller]")]
    public class UserController : RestController
    {
        private const string apiKey = "ApiKey";
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        private readonly IMemoryCache memoryCache;
        private readonly EmailSender emailSender;

        public UserController(
            IMediator mediator,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager,
            IMapper mapper,
            IConfiguration configuration,
            IMemoryCache memoryCache,
            EmailSender emailSender)
        {
            this.mediator = mediator;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.userManager = userManager;
            this.mapper = mapper;
            this.memoryCache = memoryCache;
            this.emailSender = emailSender;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Register([FromBody] RegisterCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorMessageConstants.REGISTER_UNEXPECTED);
            }

            try
            {
                User user = await mediator.Send(command);

                memoryCache.Remove(CacheKeyConstants.USERS);

                string token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                string link = $"https://localhost:{Request.Host.Port}{Url.Action(nameof(ConfirmEmail), "User", new { Token = token, Username = user.UserName })}";

                await emailSender.SendConfirmationEmailAsync(user.Email, link, user.UserName);
            }
            catch (ExistingUserRegisterException eur)
            {
                return BadRequest(eur.Message);
            }
            catch (InvalidUserCredentialsException iuc)
            {
                return BadRequest(iuc.Message);
            }
            catch (Exception)
            {
                return BadRequest(ErrorMessageConstants.REGISTER_UNEXPECTED);
            }

            return NoContent();
        }

        [HttpGet("Confirm")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ConfirmEmail([FromQuery] ConfirmEmailCommand command)
        {
            try
            {
                await mediator.Send(command);
            }
            catch (ArgumentNullException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessageConstants.INVALID_USER);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessageConstants.CONFIRM_UNEXPECTED);
            }

            return NoContent();
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthResponseModel>> Login([FromBody] LoginCommand command)
        {
            try
            {
                AuthResponseModel model = await mediator.Send(command);

                //string token = await CreateToken(command);

                return Ok(model);
            }
            catch (InvalidUserCredentialsException ae)
            {
                return BadRequest(ae.Message);
            }
            catch (InvalidOperationException io)
            {
                return BadRequest(io.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorMessageConstants.LOGIN_UNEXPECTED);
            }
        }

        [HttpGet]
        [Authorize(Roles = RoleConstants.Administrator)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ListUserModel>>> List()
        {
            try
            {
                IEnumerable<ListUserModel> users = await memoryCache.GetOrCreateAsync(CacheKeyConstants.USERS, async (entry) =>
                {
                    entry.SetSlidingExpiration(TimeSpan.FromSeconds(30));

                    return await mediator.Send(new GetAllUsersQuery());
                });

                IEnumerable<ListUserModel> outputModels = mapper.Map<IEnumerable<ListUserModel>>(users);

                AttachLinks(outputModels);

                return Ok(outputModels);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessageConstants.UNEXPECTED_ERROR);
            }
        }

        [HttpPut("{id}/Roles")]
        [Authorize(Roles = RoleConstants.Administrator)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> EditUserRoles([FromRoute] string id, [FromBody] string[] roles)
        {
            if (roles.Length == 0)
            {
                return BadRequest(ErrorMessageConstants.ROLES_EMPTY);
            }

            try
            {
                await mediator.Send(new EditUserRolesCommand(id, roles));

                return NoContent();
            }
            catch (ArgumentNullException ae)
            {
                return NotFound(ae.Message);
            }
            catch (InvalidOperationException io)
            {
                return BadRequest(io.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessageConstants.UNEXPECTED_ERROR);
            }
        }

        [Authorize]
        [HttpGet("{id}/Profile")]
        [HttpHead("{id}/Profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserProfileModel>> Profile([FromRoute] string id)
        {
            try
            {
                UserProfileModel profile = await mediator.Send(new GetProfileQuery { Id = id });

                if (profile is null)
                {
                    return NotFound();
                }

                AttachLinks(profile);

                return Ok(profile);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessageConstants.UNEXPECTED_ERROR);
            }
        }

        //To be deprecated
        [HttpPost("Roles")]
        [Authorize(Roles = RoleConstants.Administrator)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IdentityRole>> CreateRole([FromBody] string roleName)
        {
            roleName = roleName.Trim();

            if (roleName.Any(c => !char.IsWhiteSpace(c) && !char.IsLetter(c)))
            {
                return BadRequest(ErrorMessageConstants.INVALID_ROLE_INPUT);
            }

            IdentityRole role = new IdentityRole { Name = roleName };

            try
            {
                await roleManager.CreateAsync(role);

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //To be deprecated
        [HttpPost("Roles/Administrator")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateAdministratorRole()
        {
            IdentityRole role = new IdentityRole { Name = RoleConstants.Administrator };

            try
            {
                await roleManager.CreateAsync(role);

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        protected override IEnumerable<Link> GetLinks(IHypermediaResource model)
        {
            IEnumerable<Link> links = GetUserLinks(model);

            return links;
        }

        private IEnumerable<Link> GetUserLinks(IHypermediaResource resource)
        {
            IEnumerable<Link> links = new HashSet<Link>
            {
                new Link
                {
                    Url = this.GetAbsoluteAction(nameof(Profile), new {resource.Id}),
                    Rel = LinkConstants.SELF,
                    Method = HttpMethods.Get
                },
                new Link
                {
                    Url = this.GetAbsoluteAction(nameof(EditUserRoles), new {resource.Id}),
                    Rel = LinkConstants.UPDATE_ROLES,
                    Method = HttpMethods.Put
                }
            };

            return links;
        }
    }
}
