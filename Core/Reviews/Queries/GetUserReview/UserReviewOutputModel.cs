using Core.Common.Interfaces;

namespace Core.ApiModels.OutputModels.Review
{
    public class UserReviewOutputModel : IHypermediaResource
    {
        public string Id { get; set; } = null!;

        public double Value { get; set; }

        public string Comment { get; set; }

        public string BookId { get; set; }

        public IEnumerable<ILink> Links { get; set; } = new List<ILink>();
    }
}
