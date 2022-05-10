using Common;
using Common.ValidationConstants;
using Core.Services.Contracts;
using Core.ViewModels.Author;
using Core.ViewModels.Book;
using Core.ViewModels.Genre;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class BookController : Controller
    {
        private readonly IAuthorService authorService;
        private readonly IBookService bookService;
        private readonly IGenreService genreService;

        public BookController(IAuthorService authorService, IBookService bookService, IGenreService genreService)
        {
            this.authorService = authorService;
            this.bookService = bookService;
            this.genreService = genreService;
        }

        public async Task<IActionResult> All()
        {
            IEnumerable<ListBookModel> books = await bookService.GetAll();

            return View(books);
        }

        public async Task<IActionResult> Details(string id)
        {
            BookDetailsModel model = await bookService.Details(id);

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            EditBookModel model = await bookService.GetEditModel(id);
            IEnumerable<ListAuthorModel> authors = await authorService.GetAllAuthors();
            IEnumerable<ListGenreModel> genres = await genreService.GetAllGenres();
            ViewBag.Authors = authors;
            ViewBag.Genres = genres;

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            IEnumerable<ListAuthorModel> authors = await authorService.GetAllAuthors();
            IEnumerable<ListGenreModel> genres = await genreService.GetAllGenres();
            ViewBag.Authors = authors;
            ViewBag.Genres = genres;

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
                IEnumerable<ListGenreModel> genres = await genreService.GetAllGenres();
                ViewBag.Authors = authors;
                ViewBag.Genres = genres;

                return View();
            }
            if (!BookConstants.AllowedImageTypes.Contains(coverContentType))
            {
                TempData[MessageConstants.WarningMessage] = ErrorMessageConstants.COVER_ALLOWED_FORMATS;
                TempData[MessageConstants.ErrorMessage] = ErrorMessageConstants.COVER_INVALID_FORMAT;

                IEnumerable<ListAuthorModel> authors = await authorService.GetAllAuthors();
                IEnumerable<ListGenreModel> genres = await genreService.GetAllGenres();
                ViewBag.Authors = authors;
                ViewBag.Genres = genres;

                return View();
            }

            using MemoryStream stream = new MemoryStream();
            await cover.CopyToAsync(stream);
            model.Cover = stream.ToArray();

            try
            {
                await bookService.Create(model);
            }
            catch (ArgumentException ex)
            {
                TempData[MessageConstants.ErrorMessage] = string.Format(ex.Message, model.Title);
                IEnumerable<ListAuthorModel> authors = await authorService.GetAllAuthors();
                IEnumerable<ListGenreModel> genres = await genreService.GetAllGenres();
                ViewBag.Authors = authors;
                ViewBag.Genres = genres;

                return View();
            }
            catch (Exception)
            {
                TempData[MessageConstants.ErrorMessage] = ErrorMessageConstants.CREATE_BOOK_UNEXPECTED;
                IEnumerable<ListAuthorModel> authors = await authorService.GetAllAuthors();
                IEnumerable<ListGenreModel> genres = await genreService.GetAllGenres();
                ViewBag.Authors = authors;
                ViewBag.Genres = genres;

                return View();
            }

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBookModel model, IFormFile cover)
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

            try
            {
                await bookService.Edit(model);
            }
            catch (ArgumentException ex)
            {
                TempData[MessageConstants.ErrorMessage] = string.Format(ex.Message, model.Title);
                IEnumerable<ListAuthorModel> authors = await authorService.GetAllAuthors();
                ViewBag.Authors = authors;

                return View();
            }
            catch (Exception)
            {
                TempData[MessageConstants.ErrorMessage] = ErrorMessageConstants.EDIT_BOOK_UNEXPECTED;
                IEnumerable<ListAuthorModel> authors = await authorService.GetAllAuthors();
                IEnumerable<ListGenreModel> genres = await genreService.GetAllGenres();
                ViewBag.Authors = authors;
                ViewBag.Genres = genres;

                return View();
            }

            return RedirectToAction("All");
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await bookService.Delete(id);
            }
            catch (Exception)
            {
                TempData[MessageConstants.ErrorMessage] = ErrorMessageConstants.DELETE_BOOK_UNEXPECTED;
            }

            return RedirectToAction(nameof(All));
        }
    }
}
