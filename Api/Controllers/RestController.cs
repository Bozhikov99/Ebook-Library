using Core.ApiModels;

namespace Api.Controllers
{
    public abstract class RestController : ApiBaseController
    {
        protected abstract IEnumerable<HateoasLink> GetLinks(OutputModel model);
    }
}
