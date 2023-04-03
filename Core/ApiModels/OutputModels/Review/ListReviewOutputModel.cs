namespace Core.ApiModels.OutputModels.Review
{
    public class ListReviewOutputModel : OutputBaseModel
    {
        public double Value { get; set; }

        public string Comment { get; set; }

        public string UserName { get; set; }

        public string BookId { get; set; }
    }
}
