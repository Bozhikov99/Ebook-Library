using Core.ApiModels;
using Core.ApiModels.OutputModels;

namespace Api.Controllers
{
    public abstract class RestController : ApiBaseController
    {
        protected abstract IEnumerable<HateoasLink> GetLinks(OutputBaseModel model);
    }
}
