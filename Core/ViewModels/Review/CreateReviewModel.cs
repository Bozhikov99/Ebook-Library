using Common.ValidationConstants;
using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels.Review
{
    public class CreateReviewModel
    {
        [Range(ReviewConstants.VALUE_MIN, ReviewConstants.VALUE_MAX)]
        public double Value { get; set; }
        public string Comment { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string BookId { get; set; }
    }
}
