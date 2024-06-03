using Common.MessageConstants;
using Domain.Entities;
using Infrastructure.Common;
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
        private readonly IRepository repository;
        private readonly UserManager<User> userManager;

        public EditUserRolesHandler(IRepository repository, UserManager<User> userManager)
        {
            this.repository = repository;
            this.userManager = userManager;
        }

        public async Task<bool> Handle(EditUserRolesCommand request, CancellationToken cancellationToken)
        {
            string id = request.Id;
            string[] roles = request.Roles;
            bool isSuccessful = false;

            User user = await repository.GetByIdAsync<User>(id);

            ArgumentNullException.ThrowIfNull(user, ErrorMessageConstants.INVALID_USER);

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

