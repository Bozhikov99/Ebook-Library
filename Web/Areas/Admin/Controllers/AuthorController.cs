using Common.MessageConstants;
using Core.Authors.Commands.Create;
using Core.Authors.Commands.Delete;
using Core.Authors.Commands.Edit;
using Core.Authors.Queries.Common;
using Core.Authors.Queries.GetAuthors;
using Core.Authors.Queries.GetEditModel;
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

        public async Task<IActionResult> All([FromQuery] GetAuthorsQuery query)
        {
            IEnumerable<AuthorModel> authors = await mediator.Send(query);

            return View(authors);
        }

        public IActionResult Create() => View();

        public async Task<IActionResult> Edit(string id)
        {
            AuthorModel model = await mediator.Send(new GetEditAuthorModelQuery { Id = id });

            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await mediator.Send(new DeleteAuthorCommand { Id = id });
            }
            catch (Exception)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.DELETE_AUTHOR_UNEXPECTED;
            }

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAuthorCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                await mediator.Send(command);
            }
            catch (Exception)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.CREATE_AUTHOR_UNEXPECTED;
                return View();
            }

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditAuthorCommand command)
        {
            try
            {
                await mediator.Send(command);
            }
            catch (ArgumentException ae)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ae.Message;
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
