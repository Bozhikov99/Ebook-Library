using Core.Services.Contracts;
using Core.ViewModels.Genre;
using Microsoft.AspNetCore.Mvc;

namespace Web.Views.Shared.Components.BrowseGenres
{
    public class BrowseGenresViewComponent : ViewComponent
    {
        private readonly IGenreService genreService;

        public BrowseGenresViewComponent(IGenreService genreService)
        {
            this.genreService = genreService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<ListGenreModel> genres = await genreService.GetAllGenres();

            return View(genres);
        }
    }
}
