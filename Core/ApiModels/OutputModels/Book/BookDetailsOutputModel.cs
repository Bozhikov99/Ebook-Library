using Core.ApiModels.OutputModels.Review;

namespace Core.ApiModels.OutputModels.Book
{
    public class BookDetailsOutputModel : OutputBaseModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public byte[] Cover { get; set; }

        public int ReleaseYear { get; set; }

        public int Pages { get; set; }

        public string Author { get; set; }

        public string[] Genres { get; set; }

        public double Rating { get; set; }

        public string UserId { get; set; }

        public bool IsFavourite { get; set; }

        public UserReviewOutputModel UserReview { get; set; }

        public IEnumerable<ListReviewOutputModel> Reviews { get; set; }
    }
}
