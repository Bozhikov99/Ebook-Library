using Common.MessageConstants;
using Common.ValidationConstants;
using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels.Genre
{
    public class EditGenreModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(GenreConstants.NAME_MAX_LENGTH, MinimumLength = GenreConstants.NAME_MIN_LENGTH, ErrorMessage = ErrorMessageConstants.INVALID_LENGTH)]
        public string Name { get; set; }
    }
}
