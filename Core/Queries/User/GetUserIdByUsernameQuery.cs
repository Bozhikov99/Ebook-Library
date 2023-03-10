using MediatR;

namespace Core.Queries.User
{
    public class GetUserIdByUsernameQuery : IRequest<string>
    {
        public GetUserIdByUsernameQuery(string username)
        {
            Username = username;
        }

        public string Username { get; private set; }
    }
}
