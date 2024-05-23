using Api.Extenstions;
using Api.Hypermedia;
using Core.Common.Interfaces;

namespace Api.Controllers
{
    public abstract class RestController : ApiBaseController
    {
        protected void AttachLinks(IHypermediaResource outputModel)
        {
            if (this.IsHateoasRequired())
            {
                outputModel.Links = GetLinks(outputModel);
            }
        }

        protected void AttachLinks(IEnumerable<IHypermediaResource> outputModels)
        {
            if (this.IsHateoasRequired())
            {
                foreach (IHypermediaResource resource in outputModels)
                {
                    resource.Links = GetLinks(resource);
                }
            }
        }

        protected abstract IEnumerable<Link> GetLinks(IHypermediaResource model);
    }
}
