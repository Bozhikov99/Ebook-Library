using Common;
using Common.ValidationConstants;
using Core.Services.Contracts;
using Core.ViewModels.Author;
using Core.ViewModels.Book;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class BookController : Controller
    {
        private readonly IAuthorService authorService;
        private readonly IBookService bookService;

        public BookController(IAuthorService authorService, IBookService bookService)
        {
            this.authorService = authorService;
            this.bookService = bookService;
        }

        public async Task<IActionResult> Create()
        {
            IEnumerable<ListAuthorModel> authors = await authorService.GetAllAuthors();
            ViewBag.Authors = authors;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookModel model, IFormFile cover)
        {
            string coverContentType = cover.ContentType;

            if (cover == null || cover.Length == 0)
            {
                TempData[MessageConstants.ErrorMessage] = ErrorMessageConstants.COVER_ISNULL;

                IEnumerable<ListAuthorModel> authors = await authorService.GetAllAuthors();
                ViewBag.Authors = authors;

                return View();
            }
            if (!BookConstants.AllowedImageTypes.Contains(coverContentType))
            {
                TempData[MessageConstants.WarningMessage] = ErrorMessageConstants.COVER_ALLOWED_FORMATS;
                TempData[MessageConstants.ErrorMessage] = ErrorMessageConstants.COVER_INVALID_FORMAT;

                IEnumerable<ListAuthorModel> authors = await authorService.GetAllAuthors();
                ViewBag.Authors = authors;

                return View();
            }

            using MemoryStream stream = new MemoryStream();
            await cover.CopyToAsync(stream);
            model.Cover = stream.ToArray();


            if (!ModelState.IsValid)
            {
                IEnumerable<ListAuthorModel> authors = await authorService.GetAllAuthors();
                ViewBag.Authors = authors;

                return View();
            }

            try
            {
                await bookService.Create(model);
            }
            catch (Exception)
            {

                throw;
            }


            return RedirectToAction("All");
        }
    }
}
