using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels.Review
{
    public class ListReviewModel
    {
        [UIHint("Hidden")]
        public string Id { get; set; }

        public double Value { get; set; }

        public string Comment { get; set; }

        public string UserName { get; set; }

        public string BookId { get; set; }
    }
}
