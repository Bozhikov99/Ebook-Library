using Domain.Entities;
using Infrastructure.Persistance;

namespace Core.Users.Queries.GetProfile
{
    public class GetProfileQuery : IRequest<UserProfileModel>
    {
        public string Id { get; set; } = null!;
    }

    public class GetProfileHandler : IRequestHandler<GetProfileQuery, UserProfileModel>
    {
        private readonly EbookDbContext context;

        public GetProfileHandler(EbookDbContext context)
        {
            this.context = context;
        }

        public async Task<UserProfileModel> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            string userId = request.Id;

            User? user = await context.Users
                .Select(u => new User
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    RegisterDate = u.RegisterDate,
                    FavouriteBooks = u.FavouriteBooks,
                    Subscriptions = u.Subscriptions
                })
                .FirstOrDefaultAsync(u => string.Equals(u.Id, userId), cancellationToken);

            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            DateTime? subscribedDue = user.Subscriptions
                .Where(s => s.Deadline > DateTime.Now)
                .OrderByDescending(s => s.Deadline)
                .Select(s => s.Deadline)
                .FirstOrDefault();

            IEnumerable<FavouriteBookDto> favouriteBooks = user.FavouriteBooks
                .Select(b => new FavouriteBookDto
                {
                    Id = b.Id,
                    Cover = b.Cover
                });

            UserProfileModel profile = new UserProfileModel
            {
                Id = userId,
                UserName = user.UserName,
                Email = user.Email,
                RegisterDate = user.RegisterDate,
                SubscribedDueDate = subscribedDue,
                FavouriteBooks = favouriteBooks
            };

            return profile;
        }
    }
}
