using Common;
using Core.Services.Contracts;
using Core.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult Login() => View();

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            IdentityResult isRegistered = await userService.Register(model);

            if (!isRegistered.Succeeded)
            {
                TempData[MessageConstants.ErrorMessage] = "ERROR BE";
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await userService.Login(model);

            return RedirectToAction("Index", "Home");
        }
    }
}
