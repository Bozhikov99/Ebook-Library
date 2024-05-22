using Api.Extenstions;
using AutoMapper;
using Common;
using Common.ApiConstants;
using Common.MessageConstants;
using Core.ApiModels;
using Core.ApiModels.OutputModels;
using Core.Genres.Commands.Create;
using Core.Genres.Commands.Delete;
using Core.Genres.Commands.Edit;
using Core.Genres.Queries.Common;
using Core.Genres.Queries.GetEditModelQuery;
using Core.Genres.Queries.GetGenres;
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
        public async Task<ActionResult<IEnumerable<GenreModel>>> List([FromQuery] GetGenresQuery query)
        {
            try
            {
                IEnumerable<ListGenreModel> genres = await memoryCache.GetOrCreateAsync(CacheKeyConstants.GENRES, async (entry) =>
                {
                    entry.SetSlidingExpiration(TimeSpan.FromSeconds(30));

                    return await mediator.Send(query);
                });

                IEnumerable<GenreModel> outputModels = mapper.Map<IEnumerable<GenreModel>>(genres);

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
                await mediator.Send(new DeleteGenreCommand { Id = id });
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
        public async Task<ActionResult<GenreModel>> GetInputModel([FromRoute] string id)
        {
            try
            {
                GenreModel model = await mediator.Send(new GetEditModelQuery { Id = id });

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
        public async Task<ActionResult> Edit([FromRoute] string id, [FromBody] EditGenreCommand command)
        {
            try
            {
                await mediator.Send(command);
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
        public async Task<ActionResult<ListGenreModel>> Create([FromBody] CreateGenreCommand command)
        {
            try
            {
                string id = await mediator.Send(command);
                memoryCache.Remove(CacheKeyConstants.GENRES);

                return Created(nameof(Edit), id);
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
