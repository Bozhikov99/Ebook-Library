using Common.MessageConstants;
using Core.ViewModels.Book;
using Core.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Domain.Exceptions;
using MediatR;
using Core.Queries.User;
using Core.Commands.UserCommands;

namespace Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IMediator mediator;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(IMediator mediator, RoleManager<IdentityRole> roleManager)
        {
            this.mediator = mediator;
            this.roleManager = roleManager;
        }

        public IActionResult Login() => View();

        public IActionResult Register() => View();

        public async Task<IActionResult> Profile()
        {
            UserProfileModel model = await mediator.Send(new GetUserProfileQuery());
            IEnumerable<ListBookModel> books = await mediator.Send(new GetFavouriteBooksQuery());
            ViewBag.Books = books;
            ViewBag.Subscription = await mediator.Send(new GetActiveSubscriptionQuery());

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
                bool isRegistered = await mediator.Send(new RegisterCommand(model));
                if (!isRegistered)
                {
                    TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.REGISTER_UNEXPECTED;
                    return View();
                }
            }
            catch (ExistingUserRegisterException ae)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ae.Message;
                return View();
            }
            catch (Exception ex)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.REGISTER_UNEXPECTED;
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
                await mediator.Send(new LoginCommand(model));
            }
            catch (InvalidUserCredentialsException ae)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ae.Message;
                return View();
            }
            catch (Exception)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.LOGIN_UNEXPECTED;
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CreateRole()
        {
            //await roleManager.CreateAsync(new IdentityRole()
            //{
            //    Name = "Administrator"
            //});

            return Ok();
        }
    }
}
