using Common;
using Core.Helpers;
using Domain.Entities;
using Infrastructure.Common;
using Microsoft.AspNetCore.Identity;

namespace Core.Users.Queries.IsUserAdmin
{
    public class IsUserAdminQuery : IRequest<bool>
    {
        public IsUserAdminQuery()
        {
        }

        public class IsUserAdminHandler : IRequestHandler<IsUserAdminQuery, bool>
        {
            private readonly IRepository repository;
            private readonly UserManager<User> userManager;
            private readonly UserIdHelper helper;

            public IsUserAdminHandler(
                IRepository repository,
                UserManager<User> userManager,
                UserIdHelper helper)
            {
                this.repository = repository;
                this.userManager = userManager;
                this.helper = helper;
            }

            public async Task<bool> Handle(IsUserAdminQuery request, CancellationToken cancellationToken)
            {
                string userId = helper.GetUserId();

                User user = await repository.GetByIdAsync<User>(userId);
                bool isAdmin = await userManager.IsInRoleAsync(user, RoleConstants.Administrator);

                return isAdmin;
            }
        }
    }
}

