using Common.MessageConstants;
using Core.Services.Contracts;
using Core.ViewModels.Book;
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
            IEnumerable<ListBookModel> books = await userService.GetFavouriteBooks();
            ViewBag.Books = books;
            ViewBag.Subscription = await userService.GetActiveSubscription();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                IdentityResult isRegistered = await userService.Register(model);
                if (!isRegistered.Succeeded)
                {
                    TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.REGISTER_UNEXPECTED;
                    return View();
                }
            }
            catch (ArgumentException ae)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ae.Message;
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
                TempData[ToastrMessageConstants.ErrorMessage] = ae.Message;
            }
            catch (Exception)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.LOGIN_UNEXPECTED;
            }

            return View();
        }
    }
}
