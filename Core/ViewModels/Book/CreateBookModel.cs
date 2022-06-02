using Common.MessageConstants;
using Common.ValidationConstants;
using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels.Book
{
    public class CreateBookModel
    {
        [Required]
        [MaxLength(BookConstants.TITLE_MAXLENGTH, ErrorMessage = ErrorMessageConstants.INVALID_LENGTH_MAXONLY)]
        public string Title { get; set; }

        [Required]
        [StringLength(BookConstants.DESCRIPTION_MAXLENGTH, MinimumLength = BookConstants.DESCRIPTION_MINLENGTH, ErrorMessage = ErrorMessageConstants.INVALID_LENGTH)]
        public string Description { get; set; }

        public byte[] Cover { get; set; }

        public byte[] Content { get; set; }

        [Range(0, BookConstants.RELEASEYEAR_MAX)]
        public int ReleaseYear { get; set; }

        [Range(0, int.MaxValue)]
        public int Pages { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public string[] GenreIds { get; set; }
    }
}
