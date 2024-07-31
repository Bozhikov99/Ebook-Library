namespace Core.Reviews.Common
{
    public abstract class BaseReviewModel
    {
        public string? Comment { get; set; }

        public string? BookId { get; set; }

        public double Value { get; set; }
    }
}
