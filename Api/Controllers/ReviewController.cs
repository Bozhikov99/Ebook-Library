﻿using Api.Extenstions;
using Api.Hypermedia;
using AutoMapper;
using Common.ApiConstants;
using Common.MessageConstants;
using Core.ApiModels.OutputModels.Review;
using Core.Common.Interfaces;
using Core.Common.Services;
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
        private readonly CurrentUserService helper;

        public ReviewController(IMediator mediator, IMapper mapper, CurrentUserService helper)
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
        public async Task<ActionResult<ReviewModel>> GetReview([FromRoute] string id)
        {
            try
            {
                ReviewModel model = await mediator.Send(new GetReviewDetailsQuery { Id = id });
                AttachLinks(model);

                return Ok(model);
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
                    Url = this.GetAbsoluteAction(nameof(GetReview), new {resource.Id}),
                    Rel = LinkConstants.SELF,
                    Method = HttpMethods.Get
                },
                new Link
                {
                    Url = this.GetAbsoluteAction(nameof(DeleteReview), new {resource.Id}),
                    Rel = LinkConstants.DELETE,
                    Method = HttpMethods.Delete
                }
            };

            return links;
        }
    }
}
