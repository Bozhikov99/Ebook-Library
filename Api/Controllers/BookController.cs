using Api.Extenstions;
using Api.Hypermedia;
using AutoMapper;
using Common;
using Common.ApiConstants;
using Common.MessageConstants;
using Common.ValidationConstants;
using Core.Books.Commands.Create;
using Core.Books.Commands.Delete;
using Core.Books.Commands.Edit;
using Core.Books.Queries.Details;
using Core.Books.Queries.GetBookEditModel;
using Core.Books.Queries.GetBooks;
using Core.Books.Queries.GetContent;
using Core.Common.Interfaces;
using Core.Common.Services;
using Core.Users.Commands.AddBookToFavourites;
using Core.Users.Commands.RemoveBookFromFavourites;
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
        private readonly CurrentUserService userService;
        private readonly IMemoryCache memoryCache;

        public BookController(IMediator mediator, IMapper mapper, IMemoryCache memoryCache, CurrentUserService helper)
        {
            this.memoryCache = memoryCache;
            this.mediator = mediator;
            this.userService = helper;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<BookModel>>> All([FromQuery] GetBooksQuery query)
        {
            try
            {
                IEnumerable<BookModel> result = await mediator.Send(query);

                return Ok(result);
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
            string userId = userService.UserId!;

            try
            {
                BookDetailsOutputModel model = await mediator.Send(new GetBookDetailsQuery { Id = id });
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Add([FromBody] CreateBookCommand command)
        {
            if (ModelState.IsValid)
            {
                return BadRequest();
            }

            await Mediator.Send(command);
            memoryCache.Remove(CacheKeyConstants.BOOKS);

            return Created(nameof(DetailsUser), command);
        }

        [HttpGet("{id}/Edit")]
        [Authorize(Roles = RoleConstants.Administrator)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EditBookModel>> GetEditModel([FromRoute] string id)
        {
            try
            {
                EditBookModel result = await Mediator.Send(new GetBookEditModelQuery { Id = id });

                return Ok(result);
            }
            catch (ArgumentNullException)
            {
                return BadRequest(ErrorMessageConstants.BOOK_DOES_NOT_EXIST);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = RoleConstants.Administrator)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Edit([FromRoute] string id, [FromBody] EditBookCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

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
                BookDetailsOutputModel model = await mediator.Send(new GetBookDetailsQuery { Id = id });
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
                //bool isSubscribed = await mediator.Send(new IsUserSubscribedQuery());

                //if (!isSubscribed)
                //{
                //    return StatusCode(StatusCodes.Status402PaymentRequired);
                //}

                byte[] content = await memoryCache.GetOrCreateAsync(string.Format(CacheKeyConstants.READ, id), async (entry) =>
                {
                    entry.SetSlidingExpiration(TimeSpan.FromSeconds(30));

                    return await mediator.Send(new GetContentQuery { BookId = id });
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
                await mediator.Send(new DeleteBookCommand { Id = id });
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
                await mediator.Send(new AddBookToFavouritesCommand { BookId = id });
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
                await mediator.Send(new RemoveBookFromFavouritesCommand { BookId = id });
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

        //private async Task<BookInputDataModel> LoadBookInputData()
        //{
        //    BookInputDataModel model = new BookInputDataModel();

        //    model.Authors = await mediator.Send(new GetAuthorsQuery());
        //    model.Genres = await mediator.Send(new GetAllGenresQuery());

        //    return model;
        //}

        protected override IEnumerable<Link> GetLinks(IHypermediaResource model)
        {
            if (model is null)
            {
                return Enumerable.Empty<Link>();
            }

            IEnumerable<Link> links = new HashSet<Link>
            {
                new Link
                {
                    Url = this.GetAbsoluteAction(nameof(DetailsUser), new {model.Id}),
                    Rel = LinkConstants.SELF,
                    Method = HttpMethods.Get
                },
                new Link
                {
                    Url = this.GetAbsoluteAction(nameof(DetailGuest), new {model.Id}),
                    Rel = LinkConstants.SELF,
                    Method = HttpMethods.Get
                },
                new Link
                {
                    Url = this.GetAbsoluteAction(nameof(Read), new {model.Id}),
                    Rel = LinkConstants.READ,
                    Method = HttpMethods.Get
                },
                new Link
                {
                    Url = this.GetAbsoluteAction(nameof(AddToFavourites), null),
                    Rel = LinkConstants.FOLLOW,
                    Method = HttpMethods.Put
                },
                new Link
                {
                    Url = this.GetAbsoluteAction(nameof(RemoveFromFavourites), null),
                    Rel = LinkConstants.UNFOLLOW,
                    Method = HttpMethods.Put
                },
                new Link
                {
                    Url = this.GetAbsoluteAction(nameof(Delete), new {model.Id}),
                    Rel = LinkConstants.DELETE,
                    Method = HttpMethods.Delete
                },
                new Link
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
