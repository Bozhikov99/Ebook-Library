using Core.ApiModels.OutputModels.Review;
using Core.Common.Interfaces;
using Core.Reviews.Common;

namespace Core.Books.Queries.Details
{
    public class BookDetailsOutputModel : IHypermediaResource
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public byte[] Cover { get; set; } = null!;

        public int ReleaseYear { get; set; }

        public int Pages { get; set; }

        public string Author { get; set; } = null!;

        public IEnumerable<string> Genres { get; set; } = new List<string>();

        public double Rating { get; set; }

        public bool IsFavourite { get; set; }

        public UserReviewOutputModel? UserReview { get; set; }

        public IEnumerable<ReviewModel> Reviews { get; set; } = new List<ReviewModel>();

        public IEnumerable<ILink> Links { get; set; } = new List<ILink>();
    }
}
