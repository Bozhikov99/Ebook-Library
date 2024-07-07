using Common;
using Common.MessageConstants;
using Core.Genres.Commands.Create;
using Core.Genres.Commands.Delete;
using Core.Genres.Commands.Edit;
using Core.Genres.Queries.Common;
using Core.Genres.Queries.GetEditModelQuery;
using Core.Genres.Queries.GetGenres;
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

        public async Task<IActionResult> All(GetGenresQuery query)
        {
            IEnumerable<ListGenreModel> genres = await mediator.Send(query);

            return View(genres);
        }

        public async Task<IActionResult> Delete(DeleteGenreCommand command)
        {
            try
            {
                await mediator.Send(command);
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
            GenreModel model = await mediator.Send(new GetEditModelQuery { Id = id });

            return View(model);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateGenreCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                await mediator.Send(command);

                TempData[ToastrMessageConstants.SuccessMessage] = string.Format(SuccessMessageConstants.GENRE_CREATED, command.Name);
            }
            catch (ArgumentException ae)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = string.Format(ae.Message, command.Name);

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
        public async Task<IActionResult> Edit(EditGenreCommand command)
        {
            if (!ModelState.IsValid)
            {
                GenreModel originalModel = await mediator.Send(new GetEditModelQuery { Id = command.Id });
                return View(originalModel);
            }
                await mediator.Send(command);
            
            return RedirectToAction(nameof(All));
        }
    }
}
