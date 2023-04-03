using Api.Helpers;
using AutoMapper;
using Common;
using Common.MessageConstants;
using Common.ValidationConstants;
using Core.ApiModels.InputModels.Books;
using Core.ApiModels.InputModels.Review;
using Core.Commands.BookCommands;
using Core.Commands.ReviewCommands;
using Core.Commands.UserCommands;
using Core.Helpers;
using Core.Queries.Author;
using Core.Queries.Book;
using Core.Queries.Genre;
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
        private readonly IMapper mapper;
        private readonly UserIdHelper helper;

        public BookController(IMediator mediator, IMapper mapper, UserIdHelper helper)
        {
            this.mediator = mediator;
            this.helper = helper;
            this.mapper = mapper;
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
        public async Task<ActionResult<BookDetailsApiModel>> DetailsUser([FromRoute] string id)
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

        [Authorize(Roles = RoleConstants.Administrator)]
        [HttpGet("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BookInputDataModel>> Add()
        {
            BookInputDataModel model = await LoadBookInputData();

            return Ok(model);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Add([FromBody] BookInputModel model)
        {
            if (ModelState.IsValid)
            {
                return BadRequest();
            }

            IEnumerable<string> errors = BookInputValidationHelper.Validate(model);

            if (errors.Any())
            {
                return BadRequest(errors);
            }

            CreateBookModel createBookModel = mapper.Map<CreateBookModel>(model);
            await mediator.Send(new CreateBookCommand(createBookModel));

            return Created(nameof(DetailsUser), model);
        }

        [Authorize(Roles = RoleConstants.Administrator)]
        [HttpGet("{id}/Edit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EditBookResponseModel>> Edit([FromRoute] string id)
        {
            BookInputDataModel bookData = await LoadBookInputData();
            try
            {
                BookInputModel model = await mediator.Send(new GetBookInputModelQuery(id));

                EditBookResponseModel response = new EditBookResponseModel
                {
                    Id = id,
                    BookData = bookData,
                    Model = model
                };

                return Ok(response);
            }
            catch (ArgumentNullException)
            {
                return BadRequest(ErrorMessageConstants.BOOK_DOES_NOT_EXIST);
            }
        }

        [Authorize(Roles = RoleConstants.Administrator)]
        [HttpPut("{id}")]
        public async Task<ActionResult> Edit([FromRoute] string id, [FromBody] BookInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            IEnumerable<string> errors = BookInputValidationHelper.Validate(model);

            if (errors.Any())
            {
                return BadRequest(errors);
            }

            EditBookModel editModel = mapper.Map<EditBookModel>(model);
            editModel.Id = id;

            await mediator.Send(new EditBookCommand(editModel));

            return NoContent();
        }

        [HttpGet("{id}/Guest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BookDetailsApiModel>> DetailGuest([FromRoute] string id)
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
        public async Task<ActionResult> Read([FromRoute] string id)
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReview(string id)
        {
            try
            {
                string userId = helper.GetUserId();

                bool isAdmin = await mediator.Send(new IsUserAdminQuery());
                IRequest request = isAdmin ? new DeleteReviewCommand(id) : new DeleteReviewApiCommand(id, userId);

                await mediator.Send(request);
            }
            catch (NullReferenceException)
            {
                return NotFound(ErrorMessageConstants.REVIEW_NOT_FOUND);
            }
            catch (InvalidOperationException io)
            {
                return BadRequest(io.Message);
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
        public async Task<ActionResult<ListReviewModel>> AddReview([FromRoute] string bookId, [FromBody] ReviewInputModel model)
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

        private async Task<BookInputDataModel> LoadBookInputData()
        {
            BookInputDataModel model = new BookInputDataModel();

            model.Authors = await mediator.Send(new GetAllAuthorsQuery());
            model.Genres = await mediator.Send(new GetAllGenresQuery());

            return model;
        }
    }
}
