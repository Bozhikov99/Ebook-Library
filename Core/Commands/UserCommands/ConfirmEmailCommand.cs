using MediatR;

namespace Core.Commands.UserCommands
{
    public class ConfirmEmailCommand : IRequest
    {
        public string Token { get; set; } = null!;

        public string Username { get; set; } = null!;
    }
}
