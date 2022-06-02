using Common.MessageConstants;
using Common.ValidationConstants;
using Core.Services.Contracts;
using Core.ViewModels.Author;
using Core.ViewModels.Book;
using Core.ViewModels.Genre;
using Core.ViewModels.Review;
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
        private readonly IReviewService reviewService;

        public BookController(
            IAuthorService authorService,
            IBookService bookService,
            IGenreService genreService,
            IUserService userService,
            IReviewService reviewService)
        {
            this.authorService = authorService;
            this.bookService = bookService;
            this.genreService = genreService;
            this.userService = userService;
            this.reviewService = reviewService;
        }

        public async Task<IActionResult> All(string search = null, string[] genres = null)
        {
            IEnumerable<ListBookModel> books;

            if (search == null && genres.Length==0)
            {
                books = await bookService.GetAll();
            }
            else if (search == null)
            {
                books = await bookService.GetAll(genres);
            }
            else if (genres.Length == 0)
            {
                books = await bookService.GetAll(search);
            }
            else
            {
                books = await bookService.GetAll(search, genres);
            }

            return View(books);
        }

        [Authorize]
        public async Task<IActionResult> Read(string id)
        {
            bool isSubscribed = await userService.isSubscribed();

            if (!isSubscribed)
            {
                TempData[ToastrMessageConstants.WarningMessage] = "Please subscribe";
                return RedirectToAction("Subscribe", "Subscription");
            }

            byte[] content = await bookService.GetContent(id);

            return File(content, BookConstants.AllowedContentType);
        }

        public async Task<IActionResult> Details(string id)
        {
            string userId = userService.GetUserId();
            BookDetailsModel model = await bookService.Details(id);
            IEnumerable<ListReviewModel> reviews = await reviewService.GetAll(userService.GetUserId(), id);

            ViewBag.UserId = userId;
            ViewBag.Reviews = reviews;

            if (userId != null)
            {
                bool isFavouriteBook = await userService.IsBookFavourite(id);
                ViewBag.IsFavourite = isFavouriteBook;

                UserReviewModel userReview = await reviewService.GetUserReview(userService.GetUserId(), id);
                ViewBag.UserReview = userReview;
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
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.COVER_ISNULL;

                IEnumerable<ListAuthorModel> authors = await authorService.GetAllAuthors();
                IEnumerable<ListGenreModel> genres = await genreService.GetAllGenres();
                ViewBag.Authors = authors;
                ViewBag.Genres = genres;

                return View();
            }
            if (!BookConstants.AllowedImageTypes.Contains(coverContentType))
            {
                TempData[ToastrMessageConstants.WarningMessage] = ErrorMessageConstants.COVER_ALLOWED_FORMATS;
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.COVER_INVALID_FORMAT;

                IEnumerable<ListAuthorModel> authors = await authorService.GetAllAuthors();
                IEnumerable<ListGenreModel> genres = await genreService.GetAllGenres();
                ViewBag.Authors = authors;
                ViewBag.Genres = genres;

                return View();
            }

            if (content == null || content.Length == 0)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.CONTENT_ISNULL;

                IEnumerable<ListAuthorModel> authors = await authorService.GetAllAuthors();
                IEnumerable<ListGenreModel> genres = await genreService.GetAllGenres();
                ViewBag.Authors = authors;
                ViewBag.Genres = genres;

                return View();
            }
            if (contentType != BookConstants.AllowedContentType)
            {
                TempData[ToastrMessageConstants.WarningMessage] = ErrorMessageConstants.CONTENT_ALLOWED_FORMATS;
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.CONTENT_INVALID_FORMAT;

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
                TempData[ToastrMessageConstants.ErrorMessage] = string.Format(ex.Message, model.Title);
                IEnumerable<ListAuthorModel> authors = await authorService.GetAllAuthors();
                IEnumerable<ListGenreModel> genres = await genreService.GetAllGenres();
                ViewBag.Authors = authors;
                ViewBag.Genres = genres;

                return View();
            }
            catch (Exception)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.CREATE_BOOK_UNEXPECTED;
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
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.COVER_ISNULL;

                IEnumerable<ListAuthorModel> authors = await authorService.GetAllAuthors();
                ViewBag.Authors = authors;

                return View();
            }
            if (!BookConstants.AllowedImageTypes.Contains(coverContentType))
            {
                TempData[ToastrMessageConstants.WarningMessage] = ErrorMessageConstants.COVER_ALLOWED_FORMATS;
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.COVER_INVALID_FORMAT;

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
                TempData[ToastrMessageConstants.ErrorMessage] = string.Format(ex.Message, model.Title);
                IEnumerable<ListAuthorModel> authors = await authorService.GetAllAuthors();
                ViewBag.Authors = authors;

                return View();
            }
            catch (Exception)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.EDIT_BOOK_UNEXPECTED;
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
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.DELETE_BOOK_UNEXPECTED;
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateReview(CreateReviewModel model)
        {
            try
            {
                await reviewService.CreateReview(model);
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction(nameof(Details), model.BookId);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteReview(string id)
        {
            try
            {
                await reviewService.DeleteReview(id);
            }
            catch (Exception)
            {

                throw;
            }

            return Ok();
        }
    }
}
