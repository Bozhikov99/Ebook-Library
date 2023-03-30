using Common.ValidationConstants;
using System.ComponentModel.DataAnnotations;

namespace Core.ApiModels.Review
{
    public class CreateReviewApiModel
    {
        [Range(ReviewConstants.VALUE_MIN, ReviewConstants.VALUE_MAX)]
        public double Value { get; set; }
        
        public string Comment { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
