using Common.MessageConstants;
using Core.Users.Commands.ConfirmEmail;
using Core.Users.Commands.Login;
using Core.Users.Commands.Register;
using Core.Users.Queries.GetProfile;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.EmailService;

namespace Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IMediator mediator;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        private readonly EmailSender emailSender;

        public UserController(
            IMediator mediator,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager,
            EmailSender emailSender)
        {
            this.mediator = mediator;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.emailSender = emailSender;
        }

        public IActionResult Login() => View();

        public IActionResult Register() => View();

        public async Task<IActionResult> Profile(string id)
        {
            UserProfileModel model = await mediator.Send(new GetProfileQuery { Id = id });

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                User user = await mediator.Send(command);

                string token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                string link = $"https://localhost:{Request.Host.Port}{Url.Action(nameof(ConfirmEmail), "User", new { Token = token, Username = user.UserName })}";

                await emailSender.SendConfirmationEmailAsync(user.Email, link, user.UserName);
            }
            catch (ExistingUserRegisterException eu)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = eu.Message;
                return View();
            }
            catch (InvalidUserCredentialsException iuc)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = iuc.Message;
                return View(iuc.Message);
            }
            catch (Exception)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.REGISTER_UNEXPECTED;
            }


            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> ConfirmEmail([FromQuery] ConfirmEmailCommand command)
        {
            try
            {
                await mediator.Send(command);
            }
            catch (ArgumentNullException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessageConstants.INVALID_USER);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessageConstants.CONFIRM_UNEXPECTED);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                await mediator.Send(command);
            }
            catch (InvalidUserCredentialsException ae)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ae.Message;
                return View();
            }
            catch (InvalidOperationException io)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = io.Message;
                return View();
            }
            catch (Exception)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.LOGIN_UNEXPECTED;
                return View();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
