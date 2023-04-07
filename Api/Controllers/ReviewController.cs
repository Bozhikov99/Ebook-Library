using Api.Extenstions;
using AutoMapper;
using Common.ApiConstants;
using Common.MessageConstants;
using Core.ApiModels;
using Core.ApiModels.InputModels.Review;
using Core.ApiModels.OutputModels;
using Core.ApiModels.OutputModels.Review;
using Core.Commands.ReviewCommands;
using Core.Helpers;
using Core.Queries.Review;
using Core.Queries.User;
using Core.ViewModels.Review;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("[controller]s")]
    public class ReviewController : RestController
    {
        private readonly IMapper mapper;
        private readonly IMediator mediator;
        private readonly UserIdHelper helper;

        public ReviewController(IMediator mediator, IMapper mapper, UserIdHelper helper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.helper = helper;
        }

        [HttpGet("{id}")]
        [HttpHead("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ListReviewOutputModel>> GetReview([FromRoute] string id)
        {
            try
            {
                ListReviewModel model = await mediator.Send(new GetReviewQuery(id));
                ListReviewOutputModel outputModel = mapper.Map<ListReviewOutputModel>(model);
                AttachLinks(outputModel);

                return Ok(outputModel);
            }
            catch (ArgumentNullException an)
            {
                return NotFound(an.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReview(string id)
        {
            try
            {
                string userId = helper.GetUserId();

                bool isAdmin = await mediator.Send(new IsUserAdminQuery());
                IRequest request = isAdmin ? new DeleteReviewCommand(id) : new DeleteReviewApiCommand(id, userId);

                await mediator.Send(request);
            }
            catch (NullReferenceException)
            {
                return NotFound(ErrorMessageConstants.REVIEW_NOT_FOUND);
            }
            catch (InvalidOperationException io)
            {
                return BadRequest(io.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        [Authorize]
        [HttpPost("/Books/{bookId}/Reviews")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ListReviewModel>> AddReview([FromRoute] string bookId, [FromBody] ReviewInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                ListReviewModel review = await mediator.Send(new CreateReviewApiCommand(bookId, model));

                return Created(nameof(GetReview), review);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
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
                    Url = this.GetAbsoluteAction(nameof(GetReview), new {model.Id}),
                    Rel = LinkConstants.SELF,
                    Method = HttpMethods.Get
                },
                new HateoasLink
                {
                    Url = this.GetAbsoluteAction(nameof(DeleteReview), new {model.Id}),
                    Rel = LinkConstants.DELETE,
                    Method = HttpMethods.Delete
                }
            };

            return links;
        }
    }
}
