using Common;
using Common.ValidationConstants;
using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels.Genre
{
    public class ListGenreModel
    {
        [Required]
        [StringLength(GenreConstants.NAME_MAX_LENGTH, MinimumLength = GenreConstants.NAME_MIN_LENGTH, ErrorMessage = ErrorMessageConstants.GENRE_LENGTH)]
        public string Name { get; set; }
    }
}
