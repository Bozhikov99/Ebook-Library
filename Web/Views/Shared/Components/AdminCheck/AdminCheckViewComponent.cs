using Core.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Views.Shared.Components.AdminCheck
{
    public class AdminCheckViewComponent : ViewComponent
    {
        private readonly IMediator mediator;

        public AdminCheckViewComponent(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            bool isAdmin = await mediator.Send(new IsUserAdminQuery());

            return View(isAdmin);
        }
    }
}
