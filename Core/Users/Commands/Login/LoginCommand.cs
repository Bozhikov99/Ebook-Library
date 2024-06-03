using Common.MessageConstants;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.Users.Commands.Login
{
    public class LoginCommand : IRequest<AuthResponseModel>
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
    }

    public class LoginHandler : IRequestHandler<LoginCommand, AuthResponseModel>
    {
        private const string apiKey = "ApiKey";
        //private readonly IJwtProvider jwtProvider;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;

        public LoginHandler(
            //IJwtProvider jwtProvider,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            //this.jwtProvider = jwtProvider;
        }

        public async Task<AuthResponseModel> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            string password = request.Password;
            string username = request.Username;

            User? user = await userManager.FindByNameAsync(username);

            if (user is null)
            {
                throw new InvalidUserCredentialsException(ErrorMessageConstants.INVALID_USER);
            }

            bool isValidPassword = await userManager.CheckPasswordAsync(user, password);

            if (!isValidPassword)
            {
                throw new InvalidUserCredentialsException(ErrorMessageConstants.INVALID_USER);
            }

            if (!user.EmailConfirmed)
            {
                throw new InvalidOperationException(ErrorMessageConstants.EMAIL_NOT_CONFIRMERD);
            }

            await signInManager.SignInAsync(user, true);

            //string token = jwtProvider.GenerateToken(model);

            IEnumerable<string> roles = await userManager.GetRolesAsync(user);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, request.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
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

            AuthResponseModel response = new AuthResponseModel
            {
                Username = user.UserName,
                IsSuccessfull = true,
                AccessToken = jwt,
            };

            return response;
        }
    }
}

