using Common.MessageConstants;
using Common.ValidationConstants;
using System.ComponentModel.DataAnnotations;

namespace Core.ApiModels.InputModels.Genre
{
    public class UpsertGenreModel
    {
        [Required]
        [StringLength(GenreConstants.NAME_MAX_LENGTH, MinimumLength = GenreConstants.NAME_MIN_LENGTH, ErrorMessage = ErrorMessageConstants.INVALID_LENGTH)]
        public string Name { get; set; }
    }
}
