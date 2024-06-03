using Core.ViewModels.User;
using Infrastructure.Persistance;

namespace Core.Users.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<IEnumerable<ListUserModel>>
    {
    }

    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<ListUserModel>>
    {
        private readonly EbookDbContext context;

        public GetAllUsersHandler(EbookDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<ListUserModel>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<ListUserModel> users = await context.Users
                .Select(u => new ListUserModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email
                })
                .ToArrayAsync(cancellationToken);

            return users;
        }
    }
}

