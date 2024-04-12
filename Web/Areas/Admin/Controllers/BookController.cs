using Common.MessageConstants;
using Common.ValidationConstants;
using Core.Books.Commands.Create;
using Core.Books.Commands.Delete;
using Core.Books.Commands.Edit;
using Core.Commands.ReviewCommands;
using Core.Helpers;
using Core.Queries.Author;
using Core.Queries.Book;
using Core.Queries.Genre;
using Core.Queries.Review;
using Core.Queries.User;
using Core.ViewModels.Author;
using Core.ViewModels.Book;
using Core.ViewModels.Genre;
using Core.ViewModels.Review;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using GetEditModelQuery = Core.Queries.Book.GetEditModelQuery;

namespace Web.Areas.Admin.Controllers
{
    public class BookController : BaseController
    {
        private readonly IMediator mediator;
        private readonly UserIdHelper helper;

        public BookController(IMediator mediator, UserIdHelper helper)
        {
            this.mediator = mediator;
            this.helper = helper;
        }

        public async Task<IActionResult> All(
            int p = 1,
            int s = BookConstants.PAGE_SIZE,
            string search = null,
            string[] genres = null)
        {
            IEnumerable<ListBookModel> books = await mediator.Send(new GetAllBooksQuery(search, genres));

            ViewBag.PageNo = p;
            ViewBag.PageSize = s;
            ViewBag.Genres = genres;
            TempData["Search"] = search;

            int starterBook = (p - 1) * s;
            ViewBag.StarterBook = starterBook;

            return View(books);
        }

        public async Task<IActionResult> Read(string id)
        {
            byte[] content = await mediator.Send(new GetContentQuery(id));

            return File(content, BookConstants.AllowedContentType);
        }

        public async Task<IActionResult> Details(string id)
        {
            string userId = helper.GetUserId();
            BookDetailsModel model = await mediator.Send(new GetBookDetailsQuery(id));
            IEnumerable<ListReviewModel> reviews = await mediator.Send(new GetAllReviewsQuery(id, userId));

            ViewBag.UserId = userId;
            ViewBag.Reviews = reviews;

            if (userId != null)
            {
                bool isFavouriteBook = await mediator.Send(new IsBookFavouriteQuery(id));
                ViewBag.IsFavourite = isFavouriteBook;

                UserReviewModel userReview = await mediator.Send(new GetUserReviewQuery(userId, id));
                ViewBag.UserReview = userReview;
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            EditBookModel model = await mediator.Send(new GetEditModelQuery(id));
            IEnumerable<ListAuthorModel> authors = await mediator.Send(new GetAllAuthorsQuery());
            IEnumerable<ListGenreModel> genres = await mediator.Send(new GetAllGenresQuery());
            ViewBag.Authors = authors;
            ViewBag.Genres = genres;

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            IEnumerable<ListAuthorModel> authors = await mediator.Send(new GetAllAuthorsQuery());
            IEnumerable<ListGenreModel> genres = await mediator.Send(new GetAllGenresQuery());
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

                IEnumerable<ListAuthorModel> authors = await mediator.Send(new GetAllAuthorsQuery());
                IEnumerable<ListGenreModel> genres = await mediator.Send(new GetAllGenresQuery());
                ViewBag.Authors = authors;
                ViewBag.Genres = genres;

                return View();
            }
            if (!BookConstants.AllowedImageTypes.Contains(coverContentType))
            {
                TempData[ToastrMessageConstants.WarningMessage] = ErrorMessageConstants.COVER_ALLOWED_FORMATS;
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.COVER_INVALID_FORMAT;

                IEnumerable<ListAuthorModel> authors = await mediator.Send(new GetAllAuthorsQuery());
                IEnumerable<ListGenreModel> genres = await mediator.Send(new GetAllGenresQuery());
                ViewBag.Authors = authors;
                ViewBag.Genres = genres;

                return View();
            }

            if (content == null || content.Length == 0)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.CONTENT_ISNULL;

                IEnumerable<ListAuthorModel> authors = await mediator.Send(new GetAllAuthorsQuery());
                IEnumerable<ListGenreModel> genres = await mediator.Send(new GetAllGenresQuery());
                ViewBag.Authors = authors;
                ViewBag.Genres = genres;

                return View();
            }
            if (contentType != BookConstants.AllowedContentType)
            {
                TempData[ToastrMessageConstants.WarningMessage] = ErrorMessageConstants.CONTENT_ALLOWED_FORMATS;
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.CONTENT_INVALID_FORMAT;

                IEnumerable<ListAuthorModel> authors = await mediator.Send(new GetAllAuthorsQuery());
                IEnumerable<ListGenreModel> genres = await mediator.Send(new GetAllGenresQuery());
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
                await mediator.Send(new CreateBookCommand(model));
            }
            catch (ArgumentException ex)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = string.Format(ex.Message, model.Title);
                IEnumerable<ListAuthorModel> authors = await mediator.Send(new GetAllAuthorsQuery());
                IEnumerable<ListGenreModel> genres = await mediator.Send(new GetAllGenresQuery());
                ViewBag.Authors = authors;
                ViewBag.Genres = genres;

                return View();
            }
            catch (Exception)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.CREATE_BOOK_UNEXPECTED;
                IEnumerable<ListAuthorModel> authors = await mediator.Send(new GetAllAuthorsQuery());
                IEnumerable<ListGenreModel> genres = await mediator.Send(new GetAllGenresQuery());
                ViewBag.Authors = authors;
                ViewBag.Genres = genres;

                return View();
            }

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBookModel model, IFormFile cover, IFormFile content)
        {
            EditBookModel bookModel = await mediator.Send(new GetEditModelQuery(model.Id));

            if (cover == null || cover.Length == 0)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.COVER_ISNULL;

                IEnumerable<ListAuthorModel> authors = await mediator.Send(new GetAllAuthorsQuery());
                IEnumerable<ListGenreModel> genres = await mediator.Send(new GetAllGenresQuery());
                ViewBag.Authors = authors;
                ViewBag.Genres = genres;

                return View(bookModel);
            }

            string coverContentType = cover.ContentType;

            if (!BookConstants.AllowedImageTypes.Contains(coverContentType))
            {
                TempData[ToastrMessageConstants.WarningMessage] = ErrorMessageConstants.COVER_ALLOWED_FORMATS;
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.COVER_INVALID_FORMAT;

                IEnumerable<ListAuthorModel> authors = await mediator.Send(new GetAllAuthorsQuery());
                IEnumerable<ListGenreModel> genres = await mediator.Send(new GetAllGenresQuery());
                ViewBag.Authors = authors;
                ViewBag.Genres = genres;

                return View(bookModel);
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
                await mediator.Send(new EditBookCommand(model));
            }
            catch (ArgumentException ex)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = string.Format(ex.Message, model.Title);
                IEnumerable<ListAuthorModel> authors = await mediator.Send(new GetAllAuthorsQuery());
                IEnumerable<ListGenreModel> genres = await mediator.Send(new GetAllGenresQuery());
                ViewBag.Authors = authors;
                ViewBag.Genres = genres;

                return View(bookModel);
            }
            catch (Exception)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.EDIT_BOOK_UNEXPECTED;
                IEnumerable<ListAuthorModel> authors = await mediator.Send(new GetAllAuthorsQuery());
                IEnumerable<ListGenreModel> genres = await mediator.Send(new GetAllGenresQuery());
                ViewBag.Authors = authors;
                ViewBag.Genres = genres;

                return View(bookModel);
            }

            return RedirectToAction("All");
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await mediator.Send(new DeleteBookCommand(id));
            }
            catch (Exception)
            {
                TempData[ToastrMessageConstants.ErrorMessage] = ErrorMessageConstants.DELETE_BOOK_UNEXPECTED;
            }

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReview(string id)
        {
            try
            {
                await mediator.Send(new DeleteReviewCommand(id));
            }
            catch (Exception)
            {

                throw;
            }

            return Ok();
        }
    }
}
