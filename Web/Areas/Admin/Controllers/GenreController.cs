using Common;
using Common.MessageConstants;
using Core.Services.Contracts;
using Core.ViewModels.Genre;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    public class GenreController : BaseController
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
                TempData[ToastrMessageConstants.SuccessMessage] = SuccessMessageConstants.GENRE_DELETED;
            }
            catch (Exception)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.DELETE_GENRE_UNEXPECTED;
                return new EmptyResult();
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

            try
            {
                await genreService.CreateGenre(model);
                TempData[ToastrMessageConstants.SuccessMessage] = string.Format(SuccessMessageConstants.GENRE_CREATED, model.Name);
            }
            catch (ArgumentException ae)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = string.Format(ae.Message, model.Name);

                return View();
            }
            catch (Exception)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.CREATE_GENRE_UNEXPECTED;

                return View();
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
                TempData[ToastrMessageConstants.ErrorMessage] = string.Format(ae.Message, model.Name);

                EditGenreModel originalModel = await genreService.GetEditModel(model.Id);
                return View(originalModel);
            }
            catch (Exception)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.EDIT_GENRE_UNEXPECTED;

                EditGenreModel originalModel = await genreService.GetEditModel(model.Id);
                return View(originalModel);
            }

            return RedirectToAction(nameof(All));
        }
    }
}
