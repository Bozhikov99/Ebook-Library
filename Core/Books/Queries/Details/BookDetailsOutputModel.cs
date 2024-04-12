using Core.ApiModels.OutputModels;
using Core.ApiModels.OutputModels.Review;

namespace Core.Books.Queries.Details
{
    public class BookDetailsOutputModel : OutputBaseModel
    {
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public byte[] Cover { get; set; } = null!;

        public int ReleaseYear { get; set; }

        public int Pages { get; set; }

        public string Author { get; set; } = null!;

        public IEnumerable<string> Genres { get; set; } = new List<string>();

        public double Rating { get; set; }

        public string UserId { get; set; } = null!;

        public bool IsFavourite { get; set; }

        public UserReviewOutputModel? UserReview { get; set; }

        public IEnumerable<ListReviewOutputModel> Reviews { get; set; } = new List<ListReviewOutputModel>();
    }
}
