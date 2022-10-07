using Common.MessageConstants;
using Core.Commands.AuthorCommands;
using Core.Queries.Author;
using Core.ViewModels.Author;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Admin.Controllers
{
    public class AuthorController : BaseController
    {
        private readonly IMediator mediator;

        public AuthorController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IActionResult> All()
        {
            IEnumerable<ListAuthorModel> authors = await mediator.Send(new GetAllAuthorsQuery());

            return View(authors);
        }

        public IActionResult Create() => View();

        public async Task<IActionResult> Edit(string id)
        {
            EditAuthorModel model = await mediator.Send(new GetEditModelQuery(id));

            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await mediator.Send(new DeleteAuthorCommand(id));
            }
            catch (Exception)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.DELETE_AUTHOR_UNEXPECTED;
            }

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAuthorModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                await mediator.Send(new CreateAuthorCommand(model));
            }
            catch (Exception)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.CREATE_AUTHOR_UNEXPECTED;
                return View();
            }

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditAuthorModel model)
        {
            try
            {
                await mediator.Send(new EditAuthorCommand(model));
            }
            catch (ArgumentException ae)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = string.Format(ae.Message, $"{model.FirstName} {model.LastName}");
                return RedirectToAction(nameof(Edit));
            }
            catch (Exception)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.EDIT_AUTHOR_UNEXPECTED;
                return RedirectToAction(nameof(Edit));
            }

            return RedirectToAction(nameof(All));
        }
    }
}
