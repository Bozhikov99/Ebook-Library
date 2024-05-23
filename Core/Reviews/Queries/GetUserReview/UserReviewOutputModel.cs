namespace Core.ApiModels.OutputModels.Review
{
    public class UserReviewOutputModel : OutputBaseModel
    {
        public double Value { get; set; }

        public string Comment { get; set; }

        public string BookId { get; set; }
    }
}
