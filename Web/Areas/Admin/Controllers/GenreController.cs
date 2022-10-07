using Common;
using Common.MessageConstants;
using Core.Commands.GenreCommands;
using Core.Queries.Genre;
using Core.ViewModels.Genre;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    public class GenreController : BaseController
    {
        private readonly IMediator mediator;

        public GenreController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IActionResult> All()
        {
            IEnumerable<ListGenreModel> genres = await mediator.Send(new GetAllGenresQuery());

            return View(genres);
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await mediator.Send(new DeleteGenreCommand(id));
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
            EditGenreModel model = await mediator.Send(new GetEditModelQuery(id));

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
                await mediator.Send(new CreateGenreCommand(model));

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
                EditGenreModel originalModel = await mediator.Send(new GetEditModelQuery(model.Id));
                return View(originalModel);
            }

            try
            {
                await mediator.Send(new EditGenreCommand(model));
            }
            catch (ArgumentException ae)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = string.Format(ae.Message, model.Name);

                EditGenreModel originalModel = await mediator.Send(new GetEditModelQuery(model.Id));
                return View(originalModel);
            }
            catch (Exception)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.EDIT_GENRE_UNEXPECTED;

                EditGenreModel originalModel = await mediator.Send(new GetEditModelQuery(model.Id));
                return View(originalModel);
            }

            return RedirectToAction(nameof(All));
        }
    }
}
