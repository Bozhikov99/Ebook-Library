using Domain.Entities;
using Infrastructure.Common;

namespace Core.Users.Queries.GetUserIdByUsername
{
    public class GetUserIdByUsernameQuery : IRequest<string>
    {
        public GetUserIdByUsernameQuery(string username)
        {
            Username = username;
        }

        public string Username { get; private set; }
    }

    public class GetUserIdByUsernameHandler : IRequestHandler<GetUserIdByUsernameQuery, string>
    {
        private readonly IRepository repository;

        public GetUserIdByUsernameHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<string> Handle(GetUserIdByUsernameQuery request, CancellationToken cancellationToken)
        {
            string username = request.Username;

            string userId = await repository.AllReadonly<User>(u => u.UserName == username)
                .Select(u => u.Id)
                .FirstAsync();

            return userId;
        }
    }
}
