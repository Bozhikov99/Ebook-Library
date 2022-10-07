using Core.Queries.Genre;
using Core.ViewModels.Genre;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Views.Shared.Components.AdminGenresViewComponent
{
    public class AdminGenresViewComponent : ViewComponent
    {
        private IMediator mediator;

        public AdminGenresViewComponent(IMediator mediator)
        {
            this.mediator=mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<ListGenreModel> genres = await mediator.Send(new GetAllGenresQuery());
            return View(genres);
        }

    }
}
