using Api.Extenstions;
using AutoMapper;
using Common.ApiConstants;
using Common.MessageConstants;
using Core.ApiModels;
using Core.ApiModels.OutputModels;
using Core.ApiModels.OutputModels.Review;
using Core.Helpers;
using Core.Reviews.Commands.Create;
using Core.Reviews.Commands.Delete;
using Core.Reviews.Common;
using Core.Reviews.Queries.GetReviewDetails;
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
                ReviewModel model = await mediator.Send(new GetReviewDetailsQuery { Id = id });
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
                await mediator.Send(new DeleteReviewCommand { Id = id });
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
        public async Task<ActionResult<BaseReviewModel>> AddReview([FromRoute] string bookId, [FromBody] CreateReviewCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                BaseReviewModel review = await mediator.Send(command);

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
