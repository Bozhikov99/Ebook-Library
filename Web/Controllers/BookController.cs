using Common.MessageConstants;
using Common.ValidationConstants;
using Core.Commands.ReviewCommands;
using Core.Commands.UserCommands;
using Core.Helpers;
using Core.Queries.Book;
using Core.Queries.Review;
using Core.Queries.User;
using Core.ViewModels.Author;
using Core.ViewModels.Book;
using Core.ViewModels.Genre;
using Core.ViewModels.Review;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class BookController : Controller
    {
        private readonly IMediator mediator;
        private readonly UserIdHelper helper;

        public BookController(
            IMediator mediator,
            UserIdHelper helper)
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

        [Authorize]
        public async Task<IActionResult> Read(string id)
        {
            bool isSubscribed = await mediator.Send(new IsUserSubscribedQuery());

            if (!isSubscribed)
            {
                TempData[ToastrMessageConstants.WarningMessage] = "Please subscribe";
                return RedirectToAction("Subscribe", "Subscription");
            }

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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToFavourites(string id)
        {
            try
            {
                await mediator.Send(new AddBookToFavouritesCommand(id));
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
                await mediator.Send(new RemoveBookFromFavouritesCommand(id));
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
                await mediator.Send(new CreateReviewCommand(model));
            }
            catch (Exception)
            {
            }

            UserReviewModel userReview = await mediator.Send(new GetUserReviewQuery(model.UserId, model.BookId));

            return Ok(userReview);
        }

        [Authorize]
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
