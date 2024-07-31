namespace Core.Users.Commands.Login
{
    public class AuthResponseModel
    {
        public string Username { get; set; } = null!;

        public bool IsSuccessfull { get; set; }

        public string? ErrorMessage { get; set; }

        public string AccessToken { get; set; } = null!;
    }
}
