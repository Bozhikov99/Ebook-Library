using Common;
using Common.ValidationConstants;
using Core.Services.Contracts;
using Core.ViewModels.Author;
using Core.ViewModels.Book;
using Core.ViewModels.Genre;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class BookController : Controller
    {
        private readonly IAuthorService authorService;
        private readonly IBookService bookService;
        private readonly IGenreService genreService;
        private readonly IUserService userService;

        public BookController(
            IAuthorService authorService,
            IBookService bookService,
            IGenreService genreService,
            IUserService userService)
        {
            this.authorService = authorService;
            this.bookService = bookService;
            this.genreService = genreService;
            this.userService = userService;
        }

        public async Task<IActionResult> All()
        {
            IEnumerable<ListBookModel> books = await bookService.GetAll();

            return View(books);
        }

        public async Task<IActionResult> Details(string id)
        {
            bool isLoggedIn = userService.GetUserId() != null;
            BookDetailsModel model = await bookService.Details(id);

            ViewBag.IsLoggedIn = isLoggedIn;

            if (isLoggedIn)
            {
                bool isFavouriteBook = await userService.IsBookFavourite(id);
                ViewBag.IsFavourite = isFavouriteBook;
            }

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
        public async Task<IActionResult> Create(CreateBookModel model, IFormFile cover, IFormFile content)
        {
            string coverContentType = cover.ContentType;
            string contentType = content.ContentType;

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

            if (content == null || content.Length == 0)
            {
                TempData[MessageConstants.ErrorMessage] = ErrorMessageConstants.CONTENT_ISNULL;

                IEnumerable<ListAuthorModel> authors = await authorService.GetAllAuthors();
                IEnumerable<ListGenreModel> genres = await genreService.GetAllGenres();
                ViewBag.Authors = authors;
                ViewBag.Genres = genres;

                return View();
            }
            if (contentType != BookConstants.AllowedContentType)
            {
                TempData[MessageConstants.WarningMessage] = ErrorMessageConstants.CONTENT_ALLOWED_FORMATS;
                TempData[MessageConstants.ErrorMessage] = ErrorMessageConstants.CONTENT_INVALID_FORMAT;

                IEnumerable<ListAuthorModel> authors = await authorService.GetAllAuthors();
                IEnumerable<ListGenreModel> genres = await genreService.GetAllGenres();
                ViewBag.Authors = authors;
                ViewBag.Genres = genres;

                return View();
            }

            //Converting the cover to byte[]
            using MemoryStream coverStream = new MemoryStream();
            await cover.CopyToAsync(coverStream);
            model.Cover = coverStream.ToArray();

            //Converting the content to byte[] 
            using MemoryStream contentStream = new MemoryStream();
            await content.CopyToAsync(contentStream);
            model.Content = contentStream.ToArray();

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
        public async Task<IActionResult> Edit(EditBookModel model, IFormFile cover, IFormFile content)
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

            //Converting the cover to byte[]
            using MemoryStream coverStream = new MemoryStream();
            await cover.CopyToAsync(coverStream);
            model.Cover = coverStream.ToArray();

            //Converting the content to byte[] 
            using MemoryStream contentStream = new MemoryStream();
            await content.CopyToAsync(contentStream);
            model.Content = contentStream.ToArray();

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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToFavourites(string id)
        {
            try
            {
                await userService.AddBookToFavourites(id);
            }
            catch (Exception)
            {
                throw;
            }

            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RemoveFromFavourites(string id)
        {
            try
            {
                await userService.RemoveBookFromFavourites(id);
            }
            catch (Exception)
            {
                throw;
            }

            return Ok();
        }
    }
}
