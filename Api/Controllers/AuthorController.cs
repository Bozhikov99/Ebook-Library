using Api.Extenstions;
using AutoMapper;
using Common;
using Common.ApiConstants;
using Common.MessageConstants;
using Core.ApiModels;
using Core.ApiModels.InputModels.Author;
using Core.ApiModels.OutputModels;
using Core.ApiModels.OutputModels.Author;
using Core.Commands.AuthorCommands;
using Core.Queries.Author;
using Core.ViewModels.Author;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Api.Controllers
{
    [Route("[controller]s")]
    public class AuthorController : RestController
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public AuthorController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListAuthorOutputModel>>> All()
        {
            try
            {
                IEnumerable<ListAuthorModel> authors = await mediator.Send(new GetAllAuthorsQuery());
                IEnumerable<ListAuthorOutputModel> outputModels = mapper.Map<IEnumerable<ListAuthorOutputModel>>(authors);

                foreach (ListAuthorOutputModel o in outputModels)
                {
                    IEnumerable<HateoasLink> links = GetLinks(o);
                    o.Links = links;
                }

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
                await mediator.Send(new DeleteAuthorCommand(id));
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
        [Authorize(Roles = RoleConstants.Administrator)]
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
        [Authorize(Roles = RoleConstants.Administrator)]
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
        //Void AttachLinks()?
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
