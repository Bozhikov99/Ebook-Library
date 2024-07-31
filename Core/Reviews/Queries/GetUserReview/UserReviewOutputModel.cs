using Core.Common.Interfaces;
using Core.Reviews.Common;

namespace Core.ApiModels.OutputModels.Review
{
    public class UserReviewOutputModel : BaseReviewModel, IHypermediaResource
    {
        public string Id { get; set; } = null!;

        public IEnumerable<ILink> Links { get; set; } = new List<ILink>();
    }
}
