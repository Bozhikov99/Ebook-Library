using Core.Services.Contracts;
using Core.ViewModels.Genre;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class GenreController : Controller
    {
        private readonly IGenreService genreService;

        public GenreController(IGenreService genreService)
        {
            this.genreService = genreService;
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateGenreModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            //TODO: Add Toastr
            try
            {
                await genreService.CreateGenre(model);
            }
            catch (Exception)
            {

            }

            return Ok();
        }
    }
}
