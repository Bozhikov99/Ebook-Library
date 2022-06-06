using Common.MessageConstants;
using Common.ValidationConstants;
using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels.User
{
    public class RegisterUserModel
    {
        [Required]
        [StringLength(UserConstants.USERNAME_MAXLENGTH, MinimumLength = UserConstants.USERNAME_MINLENGTH, ErrorMessage = ErrorMessageConstants.INVALID_LENGTH)]
        public string UserName { get; set; }

        [Required]
        [StringLength(UserConstants.PASSWORD_MAXLENGTH, MinimumLength = UserConstants.USERNAME_MINLENGTH, ErrorMessage = ErrorMessageConstants.INVALID_LENGTH)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = ErrorMessageConstants.PASSWORDS_MUST_MATCH)]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(UserConstants.EMAIL_MAXLENGTH, MinimumLength = UserConstants.EMAIL_MINLENGTH, ErrorMessage = ErrorMessageConstants.INVALID_LENGTH)]
        public string Email { get; set; }
    }
}
