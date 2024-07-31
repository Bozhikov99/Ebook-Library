using Common.MessageConstants;
using Domain.Entities;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Identity;

namespace Core.Users.Commands.EditUserRoles
{
    public class EditUserRolesCommand : IRequest<bool>
    {
        public EditUserRolesCommand(string id, string[] roles)
        {
            Id = id;
            Roles = roles;
        }

        public string Id { get; set; }
        public string[] Roles { get; set; }
    }

    public class EditUserRolesHandler : IRequestHandler<EditUserRolesCommand, bool>
    {
        private readonly EbookDbContext context;
        private readonly UserManager<User> userManager;

        public EditUserRolesHandler(EbookDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<bool> Handle(EditUserRolesCommand request, CancellationToken cancellationToken)
        {
            string id = request.Id;
            string[] roles = request.Roles;
            bool isSuccessful = false;

            User? user = await context.Users
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            if (user is null)
            {
                throw new ArgumentNullException(ErrorMessageConstants.INVALID_USER);
            }

            IEnumerable<string> userRoles = await userManager.GetRolesAsync(user);

            await userManager.RemoveFromRolesAsync(user, userRoles);

            if (roles.Length > 0)
            {
                await userManager.AddToRolesAsync(user, roles);
            }

            isSuccessful = true;

            return isSuccessful;
        }
    }
}

