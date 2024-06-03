using Api.Extenstions;
using Api.Hypermedia;
using Common;
using Common.ApiConstants;
using Common.MessageConstants;
using Core.ApiModels;
using Core.ApiModels.OutputModels;
using Core.Authors.Commands.Create;
using Core.Authors.Commands.Delete;
using Core.Authors.Commands.Edit;
using Core.Authors.Queries.Common;
using Core.Authors.Queries.GetAuthors;
using Core.Authors.Queries.GetEditModel;
using Core.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Api.Controllers
{
    [Route("[controller]s")]
    public class AuthorController : RestController
    {
        private readonly IMemoryCache memoryCache;

        public AuthorController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorModel>>> All([FromQuery] GetAuthorsQuery query)
        {
            try
            {
                IEnumerable<AuthorModel> outputModels = await Mediator.Send(query);
                AttachLinks(outputModels);

                return Ok(outputModels);
            }
            catch (Exception)
            {
                return BadRequest(ErrorMessageConstants.UNEXPECTED_ERROR);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = RoleConstants.Administrator)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete([FromRoute] string id)
        {
            try
            {
                await Mediator.Send(new DeleteAuthorCommand { Id = id });
                memoryCache.Remove(CacheKeyConstants.AUTHORS);
            }
            catch (Exception)
            {
                return BadRequest(ErrorMessageConstants.DELETE_AUTHOR_UNEXPECTED);
            }

            return NoContent();
        }

        [HttpGet("{id}/Edit")]
        [Authorize(Roles = RoleConstants.Administrator)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AuthorModel>> GetEditModel([FromRoute] string id)
        {
            try
            {
                AuthorModel model = await Mediator.Send(new GetEditAuthorModelQuery { Id = id });

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
        [Authorize(Roles = RoleConstants.Administrator)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Edit([FromRoute] string id, [FromBody] EditAuthorCommand command)
        {
            if (!string.Equals(id, command.Id))
            {
                return BadRequest();
            }
            try
            {
                await Mediator.Send(command);
                memoryCache.Remove(CacheKeyConstants.AUTHORS);

                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (ArgumentException)
            {
                return BadRequest(string.Format(ErrorMessageConstants.AUTHOR_EXISTS, $"{command.FirstName} {command.LastName}"));
            }
            catch (Exception)
            {
                return BadRequest(ErrorMessageConstants.EDIT_AUTHOR_UNEXPECTED);
            }
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.Administrator)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthorModel>> Add([FromBody] CreateAuthorCommand command)
        {
            try
            {
                string result = await Mediator.Send(command);
                memoryCache.Remove(CacheKeyConstants.AUTHORS);

                return CreatedAtAction(nameof(GetEditModel), result);
            }
            catch (ArgumentException)
            {
                return BadRequest(string.Format(ErrorMessageConstants.AUTHOR_EXISTS, $"{command.FirstName} {command.LastName}"));
            }
            catch (Exception)
            {
                return BadRequest(ErrorMessageConstants.UNEXPECTED_ERROR);
            }
        }

        protected override IEnumerable<Link> GetLinks(IHypermediaResource resource)
        {
            if (resource is null)
            {
                return Enumerable.Empty<Link>();
            }

            IEnumerable<Link> links = new HashSet<Link>
            {
                new Link
                {
                    Url = this.GetAbsoluteAction(nameof(Delete), new {resource.Id}),
                    Rel = LinkConstants.DELETE,
                    Method = HttpMethods.Delete
                },
                new Link
                {
                    Url = this.GetAbsoluteAction(nameof(Edit), new {resource.Id}),
                    Rel = LinkConstants.UPDATE,
                    Method = HttpMethods.Put
                }
            };

            return links;
        }
    }
}
