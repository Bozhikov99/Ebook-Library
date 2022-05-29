using Common.MessageConstants;
using Core.Services.Contracts;
using Core.ViewModels.Author;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IAuthorService authorService;

        public AuthorController(IAuthorService authorService)
        {
            this.authorService = authorService;
        }

        public async Task<IActionResult> All()
        {
            IEnumerable<ListAuthorModel> authors = await authorService.GetAllAuthors();

            return View(authors);
        }

        public IActionResult Create() => View();

        public async Task<IActionResult> Edit(string id)
        {
            EditAuthorModel model = await authorService.GetEditModel(id);

            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await authorService.Delete(id);
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
                await authorService.CreateAuthor(model);
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
                await authorService.EditAuthor(model);
            }
            catch (ArgumentException ae)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = string.Format(ae.Message, $"{model.FirstName} {model.LastName}");
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
