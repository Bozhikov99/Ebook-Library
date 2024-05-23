using Core.Common.Interfaces;

namespace Core.ApiModels.OutputModels.Review
{
    public class ListReviewOutputModel : IHypermediaResource
    {
        public string Id { get; set; } = null!;

        public double Value { get; set; }

        public string? Comment { get; set; }

        public string UserName { get; set; } = null!;

        public string BookId { get; set; } = null!;

        public IEnumerable<ILink> Links { get; set; } = new List<ILink>();
    }
}
