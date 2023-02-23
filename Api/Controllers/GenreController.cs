using Common.MessageConstants;
using Core.ApiModels.Genre;
using Core.Commands.GenreCommands;
using Core.Queries.Genre;
using Core.ViewModels.Genre;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace Api.Controllers
{
    [Route("[controller]")]
    public class GenreController : ApiBaseController
    {
        private readonly IMediator mediator;

        public GenreController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ListGenreModel>>> List()
        {
            try
            {
                IEnumerable<ListGenreModel> genres = await mediator.Send(new GetAllGenresQuery());

                return Ok(genres);
            }
            catch (Exception)
            {
                return BadRequest(ErrorMessageConstants.UNEXPECTED_ERROR);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete([FromRoute] string id)
        {
            try
            {
                await mediator.Send(new DeleteGenreCommand(id));
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest(ErrorMessageConstants.DELETE_GENRE_UNEXPECTED);
            }

            return NoContent();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UpsertGenreModel>> Edit([FromRoute] string id)
        {
            try
            {
                UpsertGenreModel model = await mediator.Send(new GetUpsertModelQuery(id));

                return Ok(model);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Edit([FromRoute] string id, [FromBody] UpsertGenreModel model)
        {
            try
            {
                await mediator.Send(new EditGenreApiCommand(id, model));
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (ArgumentException)
            {
                return BadRequest(ErrorMessageConstants.GENRE_EXISTS);
            }
            catch (Exception)
            {
                return BadRequest(ErrorMessageConstants.EDIT_GENRE_UNEXPECTED);
            }

            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ListGenreModel>> Create([FromBody] UpsertGenreModel model)
        {
            try
            {
                ListGenreModel result = await mediator.Send(new CreateGenreApiCommand(model));

                return Created(nameof(Edit), result);
            }
            catch (ArgumentException)
            {
                return BadRequest(ErrorMessageConstants.GENRE_EXISTS);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
    }
}
