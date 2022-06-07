using Core.Services.Contracts;
using Core.ViewModels.Genre;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Views.Shared.Components.AdminGenresViewComponent
{
    public class AdminGenresViewComponent : ViewComponent
    {
        private IGenreService genreService;

        public AdminGenresViewComponent(IGenreService genreService)
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
