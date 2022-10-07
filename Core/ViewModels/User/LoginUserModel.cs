using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels.User
{
    public class LoginUserModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
