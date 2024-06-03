using Common.MessageConstants;
using Common.ValidationConstants;
using Core.Books.Queries.Details;
using Core.Books.Queries.GetBooks;
using Core.Books.Queries.GetContent;
using Core.Helpers;
using Core.Queries.Users;
using Core.Reviews.Commands.Create;
using Core.Reviews.Commands.Delete;
using Core.Reviews.Queries.GetUserReview;
using Core.Users.Commands.AddBookToFavourites;
using Core.Users.Commands.RemoveBookFromFavourites;
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

        public async Task<IActionResult> All(GetBooksQuery query)
        {
            IEnumerable<BookModel> books = await mediator.Send(query);

            ViewBag.PageNo = query.PageNumber;
            ViewBag.PageSize = query.PageSize;
            ViewBag.Genres = query.GenreIds;
            TempData["Search"] = query.Search;

            int starterBook = (query.PageNumber - 1) * query.PageSize;
            ViewBag.StarterBook = starterBook;

            return View(books);
        }

        [Authorize]
        public async Task<IActionResult> Read(string id)
        {
            //bool isSubscribed = await mediator.Send(new IsUserSubscribedQuery());

            //if (!isSubscribed)
            //{
            //    TempData[ToastrMessageConstants.WarningMessage] = "Please subscribe";
            //    return RedirectToAction("Subscribe", "Subscription");
            //}

            byte[] content = await mediator.Send(new GetContentQuery { BookId = id });

            return File(content, BookConstants.AllowedContentType);
        }

        public async Task<IActionResult> Details(string id)
        {
            BookDetailsOutputModel model = await mediator.Send(new GetBookDetailsQuery { Id = id });

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToFavourites(string id)
        {
            await mediator.Send(new AddBookToFavouritesCommand { BookId = id });

            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RemoveFromFavourites(string id)
        {
            await mediator.Send(new RemoveBookFromFavouritesCommand { BookId = id });

            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateReview(CreateReviewCommand command)
        {
            await mediator.Send(command);

            UserReviewModel userReview = await mediator.Send(new GetUserReviewQuery { UserId = command.UserId, BookId = command.BookId });

            return Ok(userReview);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteReview(string id)
        {
            await mediator.Send(new DeleteReviewCommand { Id = id });

            return Ok();
        }
    }
}
