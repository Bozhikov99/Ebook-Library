using Core.Services.Contracts;
using Core.ViewModels.Subscription;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionService subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            this.subscriptionService = subscriptionService;
        }

        public IActionResult Subscribe()
        {
            string userId = User.Claims.First().Value;
            ViewBag.UserId = userId;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(CreateSubscriptionModel model)
        {

            try
            {
                await subscriptionService.CreateSubscription(model);
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
