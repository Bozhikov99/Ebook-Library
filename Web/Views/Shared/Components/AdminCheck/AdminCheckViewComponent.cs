using Core.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Web.Views.Shared.Components.AdminCheck
{
    public class AdminCheckViewComponent : ViewComponent
    {
        private readonly IUserService userService;

        public AdminCheckViewComponent(IUserService userService)
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
