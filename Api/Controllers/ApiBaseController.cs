using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    public abstract class ApiBaseController : ControllerBase
    {
        private ISender mediator = null!;

        protected ISender Mediator => mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}
