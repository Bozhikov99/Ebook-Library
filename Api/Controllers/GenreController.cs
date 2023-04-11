using Api.Extenstions;
using AutoMapper;
using Common;
using Common.ApiConstants;
using Common.MessageConstants;
using Core.ApiModels;
using Core.ApiModels.InputModels.Genre;
using Core.ApiModels.OutputModels;
using Core.ApiModels.OutputModels.Genre;
using Core.Commands.GenreCommands;
using Core.Queries.Genre;
using Core.ViewModels.Genre;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Api.Controllers
{
    [Route("[controller]s")]
    public class GenreController : RestController
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IMemoryCache memoryCache;

        public GenreController(IMediator mediator, IMapper mapper, IMemoryCache memoryCache)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ListGenreOutputModel>>> List()
        {
            try
            {
                IEnumerable<ListGenreModel> genres = await memoryCache.GetOrCreateAsync(CacheKeyConstants.GENRES, async (entry) =>
                {
                    entry.SetSlidingExpiration(TimeSpan.FromSeconds(30));

                    return await mediator.Send(new GetAllGenresQuery());
                });

                IEnumerable<ListGenreOutputModel> outputModels = mapper.Map<IEnumerable<ListGenreOutputModel>>(genres);

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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete([FromRoute] string id)
        {
            try
            {
                await mediator.Send(new DeleteGenreCommand(id));
                memoryCache.Remove(CacheKeyConstants.GENRES);
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
        [Authorize(Roles = RoleConstants.Administrator)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UpsertGenreModel>> GetInputModel([FromRoute] string id)
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
        [Authorize(Roles = RoleConstants.Administrator)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Edit([FromRoute] string id, [FromBody] UpsertGenreModel model)
        {
            try
            {
                await mediator.Send(new EditGenreApiCommand(id, model));
                memoryCache.Remove(CacheKeyConstants.GENRES);
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
        [Authorize(Roles = RoleConstants.Administrator)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ListGenreModel>> Create([FromBody] UpsertGenreModel model)
        {
            try
            {
                ListGenreModel result = await mediator.Send(new CreateGenreApiCommand(model));
                memoryCache.Remove(CacheKeyConstants.GENRES);

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
