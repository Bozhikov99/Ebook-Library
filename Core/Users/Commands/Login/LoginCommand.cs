using Common.MessageConstants;
using Core.ViewModels.User;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Core.Users.Commands.Login
{
    public class LoginCommand : IRequest
    {
        public LoginCommand(LoginUserModel model)
        {
            Model = model;
        }

        public LoginUserModel Model { get; private set; }
    }

    public class LoginHandler : IRequestHandler<LoginCommand>
    {
        //private readonly IJwtProvider jwtProvider;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;

        public LoginHandler(
            //IJwtProvider jwtProvider,
            SignInManager<User> signInManager,
            UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            //this.jwtProvider = jwtProvider;
        }

        public async Task<Unit> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            LoginUserModel model = request.Model;
            string username = model.UserName;

            User? user = await userManager.FindByNameAsync(username);

            if (user is null)
            {
                throw new InvalidUserCredentialsException(ErrorMessageConstants.INVALID_USER);
            }

            bool isValidPassword = await userManager.CheckPasswordAsync(user, model.Password);

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

            return Unit.Value;
        }
    }
}

