using Core.Genres.Queries.Common;
using Core.Genres.Queries.GetGenres;
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
            IEnumerable<GenreModel> genres = await mediator.Send(new GetGenresQuery());

            return View(genres);
        }
    }
}
