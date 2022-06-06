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
        
        public async Task<IActionResult> All(
            int p = 1,
            int s = BookConstants.PAGE_SIZE,
            string search = null,
            string[] genres = null)
        {
            IEnumerable<ListBookModel> books;

            if (search == null && genres.Length == 0)
            {
                books = await bookService.GetAll(p);
            }
            else if (search == null)
            {
                books = await bookService.GetAll(p, genres);
            }
            else if (genres.Length == 0)
            {
                books = await bookService.GetAll(p, search);
            }
            else
            {
                books = await bookService.GetAll(p, search, genres);
            }

            ViewBag.PageNo = p;
            ViewBag.PageSize = s;
            ViewBag.Genres = genres;
            TempData["Search"] = search;

            int starterBook = (p - 1) * s;
            ViewBag.StarterBook = starterBook;

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
