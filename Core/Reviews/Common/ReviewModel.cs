using Core.Common.Interfaces;

namespace Core.Reviews.Common
{
    public class ReviewModel : BaseReviewModel, IHypermediaResource
    {
        public string Id { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public IEnumerable<ILink> Links { get; set; } = new List<ILink>();
    }
}
