using Common.MessageConstants;
using Core.Commands.SubscriptionCommands;
using Core.Helpers;
using Core.ViewModels.Subscription;
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
        private readonly UserIdHelper userIdHelper;

        public SubscriptionController(IMediator mediator, UserIdHelper userIdHelper)
        {
            this.mediator = mediator;
            this.userIdHelper = userIdHelper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<string> Subscribe()
        {
            string userId = userIdHelper.GetUserId();

            return Ok(userId);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Subscribe([FromBody] CreateSubscriptionModel model)
        {
            try
            {
                await mediator.Send(new CreateSubscriptionCommand(model));
            }
            catch (Exception)
            {
                return BadRequest(ErrorMessageConstants.UNEXPECTED_ERROR);
            }

            return NoContent();
        }
    }
}
