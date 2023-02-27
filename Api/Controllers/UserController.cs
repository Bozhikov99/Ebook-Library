using Common.MessageConstants;
using Core.Commands.UserCommands;
using Core.ViewModels.User;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public UserController(IMediator mediator, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.mediator = mediator;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Register([FromBody] RegisterUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
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
        public async Task<ActionResult> Login(LoginUserModel model)
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

            string token = CreateToken(model);

            return Ok(token);
        }


        //move this in Core
        private string CreateToken(LoginUserModel model)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.UserName)
            };

            //SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            //    configuration.GetSection("AppSettings:Token").Value));
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                configuration.GetValue<string>(apiKey)));

            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            DateTime expiration = DateTime.Now.AddDays(1);

            JwtSecurityToken token = new JwtSecurityToken(claims: claims, expires: expiration, signingCredentials: credentials);

            string jwt = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return jwt;
        }
    }
}
