using Api.Extenstions;
using Api.Helpers;
using AutoMapper;
using Common;
using Common.ApiConstants;
using Common.MessageConstants;
using Common.ValidationConstants;
using Core.ApiModels;
using Core.ApiModels.InputModels.Books;
using Core.ApiModels.OutputModels;
using Core.ApiModels.ResponseModels;
using Core.Books.Commands.Create;
using Core.Books.Commands.Delete;
using Core.Books.Commands.Edit;
using Core.Books.Queries.Details;
using Core.Commands.UserCommands;
using Core.Helpers;
using Core.Queries.Author;
using Core.Queries.Book;
using Core.Queries.Genre;
using Core.Queries.User;
using Core.ViewModels.Book;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Api.Controllers
{
    [Route("[controller]s")]
    public class BookController : RestController
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly UserIdHelper helper;
        private readonly IMemoryCache memoryCache;

        public BookController(IMediator mediator, IMapper mapper, IMemoryCache memoryCache, UserIdHelper helper)
        {
            this.memoryCache = memoryCache;
            this.mediator = mediator;
            this.helper = helper;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BookBrowsingModel>> All([FromQuery] string? search, [FromQuery] string[]? genres)
        {
            try
            {
                BookBrowsingModel books = await memoryCache.GetOrCreateAsync(CacheKeyConstants.BOOKS, async (entry) =>
                {
                    entry.SetSlidingExpiration(TimeSpan.FromSeconds(30));

                    return await mediator.Send(new GetAllBooksApiQuery(search, genres));
                });

                books.Books
                    .ToList()
                    .ForEach(b => AttachLinks(b));

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
        public async Task<ActionResult<BookDetailsOutputModel>> DetailsUser([FromRoute] string id)
        {
            string userId = helper.GetUserId();

            try
            {
                BookDetailsOutputModel model = await mediator.Send(new GetDetailsQuery(id));
                AttachLinks(model);

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
            memoryCache.Remove(CacheKeyConstants.BOOKS);

            return Created(nameof(DetailsUser), model);
        }

        [Authorize(Roles = RoleConstants.Administrator)]
        [HttpGet("{id}/Edit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EditBookResponseModel>> GetEditModel([FromRoute] string id)
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
            memoryCache.Remove(CacheKeyConstants.BOOKS);
            memoryCache.Remove(string.Format(CacheKeyConstants.READ, id));

            return NoContent();
        }

        [HttpGet("{id}/Guest")]
        [HttpHead("{id}/Guest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BookDetailsOutputModel>> DetailGuest([FromRoute] string id)
        {
            try
            {
                BookDetailsOutputModel model = await mediator.Send(new GetGuestBookDetailsApiQuery(id));
                AttachLinks(model);

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

                byte[] content = await memoryCache.GetOrCreateAsync(string.Format(CacheKeyConstants.READ, id), async (entry) =>
                {
                    entry.SetSlidingExpiration(TimeSpan.FromSeconds(30));

                    return await mediator.Send(new GetContentQuery(id));
                });

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
                memoryCache.Remove(CacheKeyConstants.BOOKS);
                memoryCache.Remove(string.Format(CacheKeyConstants.READ, id));

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
        [HttpPut("Follow")]
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
        [HttpPut("Unfollow")]
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

        private async Task<BookInputDataModel> LoadBookInputData()
        {
            BookInputDataModel model = new BookInputDataModel();

            model.Authors = await mediator.Send(new GetAllAuthorsQuery());
            model.Genres = await mediator.Send(new GetAllGenresQuery());

            return model;
        }

        protected override IEnumerable<HateoasLink> GetLinks(OutputBaseModel model)
        {
            if (model is null)
            {
                return Enumerable.Empty<HateoasLink>();
            }

            IEnumerable<HateoasLink> links = new HashSet<HateoasLink>
            {
                new HateoasLink
                {
                    Url = this.GetAbsoluteAction(nameof(DetailsUser), new {model.Id}),
                    Rel = LinkConstants.SELF,
                    Method = HttpMethods.Get
                },
                new HateoasLink
                {
                    Url = this.GetAbsoluteAction(nameof(DetailGuest), new {model.Id}),
                    Rel = LinkConstants.SELF,
                    Method = HttpMethods.Get
                },
                new HateoasLink
                {
                    Url = this.GetAbsoluteAction(nameof(Read), new {model.Id}),
                    Rel = LinkConstants.READ,
                    Method = HttpMethods.Get
                },
                new HateoasLink
                {
                    Url = this.GetAbsoluteAction(nameof(AddToFavourites), null),
                    Rel = LinkConstants.FOLLOW,
                    Method = HttpMethods.Put
                },
                new HateoasLink
                {
                    Url = this.GetAbsoluteAction(nameof(RemoveFromFavourites), null),
                    Rel = LinkConstants.UNFOLLOW,
                    Method = HttpMethods.Put
                },
                new HateoasLink
                {
                    Url = this.GetAbsoluteAction(nameof(Delete), new {model.Id}),
                    Rel = LinkConstants.DELETE,
                    Method = HttpMethods.Delete
                },
                new HateoasLink
                {
                    Url = this.GetAbsoluteAction(nameof(Edit), new {model.Id}),
                    Rel = LinkConstants.UPDATE,
                    Method = HttpMethods.Put
                }
            };

            return links;
        }
    }
}
