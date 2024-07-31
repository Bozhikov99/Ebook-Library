using Common.MessageConstants;
using Core.Common.Services;
using Core.Subscriptions.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [Route("[controller]s")]
    public class SubscriptionController : ApiBaseController
    {
        private readonly IMediator mediator;
        private readonly CurrentUserService userService;

        public SubscriptionController(IMediator mediator, CurrentUserService userService)
        {
            this.mediator = mediator;
            this.userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<string> Subscribe()
        {
            string userId = userService.UserId!;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            return Ok(userId);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Subscribe([FromBody] SubscribeCommand commend)
        {
            try
            {
                await mediator.Send(commend);
            }
            catch (Exception)
            {
                return BadRequest(ErrorMessageConstants.UNEXPECTED_ERROR);
            }

            return NoContent();
        }
    }
}
