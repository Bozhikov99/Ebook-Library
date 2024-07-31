using Common;
using Common.MessageConstants;
using Core.Helpers;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Views.Shared.Components.AdminCheck
{
    public class AdminCheckViewComponent : ViewComponent
    {
        private readonly UserIdHelper userIdHelper;
        private readonly UserManager<User> userManager;

        public AdminCheckViewComponent(UserManager<User> userManager, UserIdHelper userIdHelper)
        {
            this.userManager = userManager;
            this.userIdHelper = userIdHelper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string userId = userIdHelper.GetUserId();

            User? user = await userManager.FindByIdAsync(userId);

            if (user is null)
            {
                throw new ArgumentNullException(ErrorMessageConstants.INVALID_USER);
            }

            bool isAdmin = await userManager.IsInRoleAsync(user, RoleConstants.Administrator);

            return View(isAdmin);
        }
    }
}
