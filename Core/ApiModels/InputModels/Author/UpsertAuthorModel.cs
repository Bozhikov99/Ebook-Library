using Common.MessageConstants;
using Common.ValidationConstants;
using System.ComponentModel.DataAnnotations;

namespace Core.ApiModels.InputModels.Author
{
    public class UpsertAuthorModel
    {
        [Required]
        [RegularExpression(AuthorConstants.AUTHOR_NAME_REGEX)]
        [StringLength(AuthorConstants.AUTHOR_NAME_MAXLENGTH, MinimumLength = AuthorConstants.AUTHOR_NAME_MINLENGTH, ErrorMessage = ErrorMessageConstants.INVALID_LENGTH)]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(AuthorConstants.AUTHOR_NAME_REGEX)]
        [StringLength(AuthorConstants.AUTHOR_NAME_MAXLENGTH, MinimumLength = AuthorConstants.AUTHOR_NAME_MINLENGTH, ErrorMessage = ErrorMessageConstants.INVALID_LENGTH)]
        public string LastName { get; set; }
    }
}
