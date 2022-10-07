using Core.Queries.Genre;
using Core.Queries.User;
using Core.ViewModels.Genre;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Views.Shared.Components.BrowseGenres
{
    public class BrowseGenresViewComponent : ViewComponent
    {
        private readonly IMediator mediator;

        public BrowseGenresViewComponent(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<ListGenreModel> genres = await mediator.Send(new GetAllGenresQuery());

            return View(genres);
        }
    }
}
