using Core.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Web.Views.Shared.Components.AdminCheckMobile
{
    public class AdminCheckMobileViewComponent : ViewComponent
    {
        private readonly IUserService userService;

        public AdminCheckMobileViewComponent(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            bool isAdmin = await userService.IsAdmin();

            return View(isAdmin);
        }
    }
}
