using Core.ApiModels;
using Core.ApiModels.OutputModels;

namespace Api.Controllers
{
    public abstract class RestController : ApiBaseController
    {
        protected void AttachLinks(OutputBaseModel outputModel)
        {
            outputModel.Links = GetLinks(outputModel);
        }

        protected void AttachLinks(IEnumerable<OutputBaseModel> outputModels)
        {
            foreach (OutputBaseModel o in outputModels)
            {
                o.Links = GetLinks(o);
            }
        }

        protected abstract IEnumerable<HateoasLink> GetLinks(OutputBaseModel model);
    }
}
