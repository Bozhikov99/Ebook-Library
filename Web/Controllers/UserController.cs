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

        public async Task<IActionResult> Profile()
        {
            UserProfileModel model = await userService.GetProfile();

            return View(model);
        }

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
                TempData[MessageConstants.ErrorMessage] = ErrorMessageConstants.REGISTER_UNEXPECTED;
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

            try
            {
                await userService.Login(model);
            }
            catch (ArgumentException ae)
            {
                TempData[MessageConstants.ErrorMessage] = ae.Message;
            }
            catch (Exception)
            {
                TempData[MessageConstants.ErrorMessage] = ErrorMessageConstants.LOGIN_UNEXPECTED;
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
