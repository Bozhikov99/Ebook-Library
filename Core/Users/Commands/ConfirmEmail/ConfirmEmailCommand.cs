using Domain.Entities;
using Infrastructure.Common;
using Microsoft.AspNetCore.Identity;

namespace Core.Users.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest
    {
        public string Token { get; set; } = null!;

        public string Username { get; set; } = null!;
    }

    public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand>
    {
        private readonly IRepository repository;
        private readonly UserManager<User> userManager;

        public ConfirmEmailHandler(IRepository repository, UserManager<User> userManager)
        {
            this.repository = repository;
            this.userManager = userManager;
        }

        public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            string token = request.Token;
            string username = request.Username;

            User user = await repository.FirstAsync<User>(u => u.UserName == username);

            await userManager.ConfirmEmailAsync(user, token);

            return Unit.Value;
        }
    }
}
