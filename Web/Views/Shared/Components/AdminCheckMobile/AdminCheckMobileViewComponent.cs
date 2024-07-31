using Common;
using Common.MessageConstants;
using Core.Common.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Views.Shared.Components.AdminCheckMobile
{
    public class AdminCheckMobileViewComponent : ViewComponent
    {
        private readonly CurrentUserService userService;
        private readonly UserManager<User> userManager;

        public AdminCheckMobileViewComponent(UserManager<User> userManager, CurrentUserService userService)
        {
            this.userManager = userManager;
            this.userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string userId = userService.UserId!;

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
