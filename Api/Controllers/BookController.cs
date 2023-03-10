using Common.MessageConstants;
using Core.ApiModels.Books;
using Core.Queries.Book;
using Core.ViewModels.Book;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("[controller]s")]
    public class BookController : ApiBaseController
    {
        private readonly IMediator mediator;

        public BookController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ListBookModel>>> All([FromQuery] string? search, [FromQuery] string[]? genres)
        {
            try
            {
                BooksModel books = await mediator.Send(new GetAllBooksApiQuery(search, genres));

                return Ok(books);
            }
            catch (Exception)
            {
                return BadRequest(ErrorMessageConstants.UNEXPECTED_ERROR);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Details(string id)
        {
            return Ok();
        }
    }
}
