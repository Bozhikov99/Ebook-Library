using Core.ViewModels.User;
using MediatR;

namespace Core.Commands.UserCommands
{
    public class AuthenticateApiCommand : IRequest<string>
    {
        public AuthenticateApiCommand(LoginUserModel credentials)
        {
            Credentials = credentials;
        }

        public LoginUserModel Credentials { get; set; }
    }
}
