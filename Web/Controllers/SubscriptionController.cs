using Common.MessageConstants;
using Core.Commands.SubscriptionCommands;
using Core.ViewModels.Subscription;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class SubscriptionController : Controller
    {
        private readonly IMediator mediator;

        public SubscriptionController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public IActionResult Subscribe()
        {
            string userId = User.Claims
                .First()
                .Value;
            
            ViewBag.UserId = userId;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(CreateSubscriptionModel model)
        {
            try
            {
                await mediator.Send(new CreateSubscriptionCommand(model));
            }
            catch (Exception)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.UNEXPECTED_ERROR;
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
