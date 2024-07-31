using Common.MessageConstants;
using Domain.Entities;
using Infrastructure.Persistance;
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
        private readonly EbookDbContext context;
        private readonly UserManager<User> userManager;

        public ConfirmEmailHandler(EbookDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            string token = request.Token;
            string username = request.Username;

            User? user = await context.Users
                .FirstOrDefaultAsync(u => u.UserName == username);

            if (user is null)
            {
                throw new ArgumentNullException(ErrorMessageConstants.INVALID_USER);
            }

            await userManager.ConfirmEmailAsync(user, token);

            return Unit.Value;
        }
    }
}
