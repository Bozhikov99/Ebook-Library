using Api.Extenstions;
using AutoMapper;
using Common;
using Common.ApiConstants;
using Common.MessageConstants;
using Core.ApiModels;
using Core.ApiModels.InputModels.Author;
using Core.ApiModels.OutputModels;
using Core.Authors.Commands.Create;
using Core.Authors.Commands.Delete;
using Core.Authors.Commands.Edit;
using Core.Authors.Queries.Common;
using Core.Authors.Queries.GetAuthors;
using Core.Queries.Author;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Api.Controllers
{
    [Route("[controller]s")]
    public class AuthorController : RestController
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IMemoryCache memoryCache;

        public AuthorController(IMediator mediator, IMapper mapper, IMemoryCache memoryCache)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorModel>>> All([FromQuery] GetAuthorsQuery query)
        {
            try
            {
                //IEnumerable<Core.ViewModels.Author.ListAuthorModel> authors = await memoryCache.GetOrCreateAsync(CacheKeyConstants.AUTHORS, async (entry) =>
                //{
                //    entry.SetSlidingExpiration(TimeSpan.FromSeconds(30));

                //    return await mediator.Send(new GetAllAuthorsQuery());
                //});

                //IEnumerable<Core.ApiModels.OutputModels.Author.ListAuthorModel> outputModels = mapper.Map<IEnumerable<Core.ApiModels.OutputModels.Author.ListAuthorModel>>(authors);

                IEnumerable<AuthorModel> outputModels = await mediator.Send(query);
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
                await mediator.Send(new DeleteAuthorCommand { Id = id });
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
        public async Task<ActionResult<UpsertAuthorModel>> GetEditModel([FromRoute] string id)
        {
            try
            {
                AuthorModel model = await mediator.Send(new GetEditAuthorApiQuery { Id = id });

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
            try
            {
                await mediator.Send(command);
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
