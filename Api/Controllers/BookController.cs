using Common;
using Common.MessageConstants;
using Common.ValidationConstants;
using Core.ApiModels.Books;
using Core.ApiModels.Review;
using Core.Commands.BookCommands;
using Core.Commands.ReviewCommands;
using Core.Commands.UserCommands;
using Core.Helpers;
using Core.Queries.Book;
using Core.Queries.Review;
using Core.Queries.User;
using Core.ViewModels.Book;
using Core.ViewModels.Review;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("[controller]s")]
    public class BookController : ApiBaseController
    {
        private readonly IMediator mediator;
        private readonly UserIdHelper helper;

        public BookController(IMediator mediator, UserIdHelper helper)
        {
            this.mediator = mediator;
            this.helper = helper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ListBookModel>>> All([FromQuery] string? search, [FromQuery] string[]? genres)
        {
            try
            {
                BooksBrowsingModel books = await mediator.Send(new GetAllBooksApiQuery(search, genres));

                return Ok(books);
            }
            catch (Exception)
            {
                return BadRequest(ErrorMessageConstants.UNEXPECTED_ERROR);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DetailsUser([FromRoute] string id)
        {
            string userId = helper.GetUserId();

            try
            {
                BookDetailsApiModel model = await mediator.Send(new GetUserBookDetailsApiQuery(id, userId));

                return Ok(model);
            }
            catch (InvalidOperationException io)
            {
                return NotFound(ErrorMessageConstants.BOOK_DOES_NOT_EXIST);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}/Guest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DetailGuest([FromRoute] string id)
        {
            try
            {
                BookDetailsApiModel model = await mediator.Send(new GetGuestBookDetailsApiQuery(id));

                return Ok(model);
            }
            catch (ArgumentNullException)
            {
                return NotFound(ErrorMessageConstants.BOOK_DOES_NOT_EXIST);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpGet("{id}/Read")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status402PaymentRequired)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Read([FromRoute] string id)
        {
            try
            {
                bool isSubscribed = await mediator.Send(new IsUserSubscribedQuery());

                if (!isSubscribed)
                {
                    return StatusCode(StatusCodes.Status402PaymentRequired);
                }

                byte[] content = await mediator.Send(new GetContentQuery(id));

                return File(content, BookConstants.AllowedContentType);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete([FromRoute] string id)
        {
            try
            {
                await mediator.Send(new DeleteBookCommand(id));

                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return NotFound(ErrorMessageConstants.BOOK_DOES_NOT_EXIST);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessageConstants.DELETE_BOOK_UNEXPECTED);
            }
        }

        #region [Favourite]

        [Authorize]
        [HttpPut("/Follow")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddToFavourites([FromBody] string id)
        {
            try
            {
                await mediator.Send(new AddBookToFavouritesCommand(id));
            }
            catch (ArgumentNullException)
            {
                return NotFound(ErrorMessageConstants.BOOK_DOES_NOT_EXIST);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        [Authorize]
        [HttpPut("/Unfollow")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveFromFavourites([FromBody] string id)
        {
            try
            {
                await mediator.Send(new RemoveBookFromFavouritesCommand(id));
            }
            catch (ArgumentNullException)
            {
                return NotFound(ErrorMessageConstants.BOOK_DOES_NOT_EXIST);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        #endregion

        #region [Review]

        [HttpGet("Reviews/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ListReviewModel>> GetReview([FromRoute] string id)
        {
            try
            {
                ListReviewModel model = await mediator.Send(new GetReviewQuery(id));

                return Ok(model);
            }
            catch (ArgumentNullException an)
            {
                return NotFound(an.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpDelete("Reviews/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReview(string id)
        {
            try
            {
                await mediator.Send(new DeleteReviewCommand(id));
            }
            catch (NullReferenceException)
            {
                return NotFound(ErrorMessageConstants.REVIEW_NOT_FOUND);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        [Authorize]
        [HttpPost("{bookId}/Reviews")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ListReviewModel>> AddReview([FromRoute] string bookId, [FromBody] CreateReviewApiModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                ListReviewModel review = await mediator.Send(new CreateReviewApiCommand(bookId, model));

                return Created(nameof(GetReview), review);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #endregion
    }
}
