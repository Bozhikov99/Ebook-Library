using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Common.MessageConstants;
using Core.ApiModels.Author;
using Core.Commands.AuthorCommands;
using Core.Queries.Author;
using Core.ViewModels.Author;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("[controller]s")]
    public class AuthorController : ApiBaseController
    {
        private readonly IMediator mediator;

        public AuthorController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListAuthorModel>>> All()
        {
            try
            {
                IEnumerable<ListAuthorModel> authors = await mediator.Send(new GetAllAuthorsQuery());

                return Ok(authors);
            }
            catch (Exception)
            {
                return BadRequest(ErrorMessageConstants.UNEXPECTED_ERROR);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete([FromRoute] string id)
        {
            try
            {
                await mediator.Send(new DeleteAuthorCommand(id));
            }
            catch (Exception)
            {
                return BadRequest(ErrorMessageConstants.DELETE_AUTHOR_UNEXPECTED);
            }

            return NoContent();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UpsertAuthorModel>> Edit([FromRoute] string id)
        {
            try
            {
                UpsertAuthorModel model = await mediator.Send(new GetEditAuthorApiQuery(id));

                return Ok(model);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest(ErrorMessageConstants.UNEXPECTED_ERROR);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Edit([FromRoute] string id, [FromBody] UpsertAuthorModel model)
        {
            try
            {
                await mediator.Send(new EditAuthorApiCommand(id, model));

                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (ArgumentException)
            {
                return BadRequest(string.Format(ErrorMessageConstants.AUTHOR_EXISTS, $"{model.FirstName} {model.LastName}"));
            }
            catch (Exception)
            {
                return BadRequest(ErrorMessageConstants.EDIT_AUTHOR_UNEXPECTED);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ListAuthorModel>> Add([FromBody] UpsertAuthorModel model)
        {
            try
            {
                ListAuthorModel result = await mediator.Send(new CreateAuthorApiCommand(model));

                return Created(nameof(Edit), result);
            }
            catch (ArgumentException)
            {
                return BadRequest(string.Format(ErrorMessageConstants.AUTHOR_EXISTS, $"{model.FirstName} {model.LastName}"));
            }
            catch (Exception)
            {
                return BadRequest(ErrorMessageConstants.UNEXPECTED_ERROR);
            }
        }
    }
}
