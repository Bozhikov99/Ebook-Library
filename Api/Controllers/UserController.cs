using Common;
using Common.MessageConstants;
using Core.ApiModels.User;
using Core.Commands.UserCommands;
using Core.Queries.User;
using Core.ViewModels.Book;
using Core.ViewModels.Subscription;
using Core.ViewModels.User;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers
{
    [Route("[controller]")]
    public class UserController : ApiBaseController
    {
        private const string apiKey = "ApiKey";
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;

        public UserController(
            IMediator mediator,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager,
            IConfiguration configuration)
        {
            this.mediator = mediator;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.userManager = userManager;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Register([FromBody] RegisterUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorMessageConstants.REGISTER_UNEXPECTED);
            }

            try
            {
                bool isRegistered = await mediator.Send(new RegisterCommand(model));

                if (!isRegistered)
                {
                    return BadRequest(ErrorMessageConstants.REGISTER_UNEXPECTED);
                }
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

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> Login([FromBody] LoginUserModel model)
        {
            try
            {
                await mediator.Send(new LoginCommand(model));
            }
            catch (InvalidUserCredentialsException ae)
            {
                return BadRequest(ErrorMessageConstants.INVALID_USER);
            }
            catch (Exception)
            {
                return BadRequest(ErrorMessageConstants.LOGIN_UNEXPECTED);
            }

            string token = await CreateToken(model);

            return Ok(token);
        }

        [HttpGet]
        [Authorize(Roles = RoleConstants.Administrator)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ListUserModel>>> ManageUsers()
        {
            try
            {
                IEnumerable<ListUserModel> users = await mediator.Send(new GetAllUsersQuery());

                return Ok(users);
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
                await mediator.Send(new EditRolesCommand(id, roles));

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
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("Profile")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProfileModel>> Profile()
        {
            try
            {
                UserProfileModel profile = await mediator.Send(new GetUserProfileQuery());

                if (profile is null)
                {
                    return NotFound();
                }

                IEnumerable<ListBookModel> books = await mediator.Send(new GetFavouriteBooksQuery());
                ListSubscriptionModel Subscription = await mediator.Send(new GetActiveSubscriptionQuery());

                ProfileModel model = new ProfileModel
                {
                    Email = profile.Email,
                    UserName = profile.UserName,
                    RegisterDate = profile.RegisterDate,
                    Books = books,
                    Subscription = Subscription
                };

                return Ok(model);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("Roles/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<RoleInfoModel>>> GetRole([FromRoute] string id)
        {
            try
            {
                RoleInfoModel role = await mediator.Send(new GetRoleQuery(id));

                return Ok(role);
            }
            catch (ArgumentNullException an)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, an.Message);
            }
        }

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

                return Created(nameof(GetRole), role);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

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

                return Created(nameof(GetRole), role);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //move this in Core
        private async Task<string> CreateToken(LoginUserModel model)
        {
            string userId = await mediator.Send(new GetUserIdByUsernameQuery(model.UserName));
            User user = await userManager.FindByIdAsync(userId);

            IEnumerable<string> roles = await userManager.GetRolesAsync(user);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.UserName),
                new Claim(ClaimTypes.NameIdentifier, userId)
            };

            foreach (string r in roles)
            {
                Claim current = new Claim(ClaimTypes.Role, r);
                claims.Add(current);
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                configuration.GetValue<string>(apiKey)));

            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            DateTime expiration = DateTime.Now.AddDays(1);

            JwtSecurityToken token = new JwtSecurityToken(
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            string jwt = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return jwt;
        }
    }
}
