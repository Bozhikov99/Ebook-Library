using Common.MessageConstants;
using Common.ValidationConstants;
using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels.Genre
{
    public class CreateGenreModel
    {
        [Required]
        [StringLength(GenreConstants.NAME_MAX_LENGTH, MinimumLength = GenreConstants.NAME_MIN_LENGTH, ErrorMessage = ErrorMessageConstants.INVALID_LENGTH)]
        public string Name { get; set; }
    }
}