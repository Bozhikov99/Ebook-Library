using Common;
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

        public async Task<IActionResult> All()
        {
            IEnumerable<ListGenreModel> genres = await genreService.GetAllGenres();

            return View(genres);
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await genreService.DeleteGenre(id);
            }
            catch (Exception)
            {
                return Ok();
            }

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Edit(string id)
        {
            EditGenreModel model = await genreService.GetEditModel(id);

            return View(model);
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
            catch (ArgumentException ae)
            {
                return Ok(string.Format(ae.Message, model.Name));
            }
            catch (Exception)
            {
                return Ok(ErrorMessageConstants.CREATE_GENRE_UNEXPECTED);
            }

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditGenreModel model)
        {
            if (!ModelState.IsValid)
            {
                EditGenreModel originalModel = await genreService.GetEditModel(model.Id);
                return View(originalModel);
            }

            try
            {
                await genreService.EditGenre(model);
            }
            catch (ArgumentException ae)
            {
                return Ok(string.Format(ae.Message, model.Name));
            }
            catch(Exception)
            {
                return Ok(ErrorMessageConstants.EDIT_GENRE_UNEXPECTED);
            }

            return RedirectToAction(nameof(All));
        }
    }
}
