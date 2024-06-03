using Core.Helpers;
using Infrastructure.Persistance;

namespace Core.Users.Queries.GetUserProfile
{
    public class GetUserProfileQuery : IRequest<UserProfileModel>
    {
    }

    public class GetUserProfileHandler : IRequestHandler<GetUserProfileQuery, UserProfileModel>
    {
        private readonly EbookDbContext context;
        private readonly UserIdHelper helper;

        public GetUserProfileHandler(EbookDbContext context, UserIdHelper helper)
        {
            this.context = context;
            this.helper = helper;
        }

        public async Task<UserProfileModel> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            string userId = helper.GetUserId();

            UserProfileModel? user = await context.Users
                .Select(u => new UserProfileModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    RegisterDate = u.RegisterDate,
                    Subscription = u.Subscriptions
                        .Where(s => s.Deadline > DateTime.Now)
                        .Select(s => new ActiveSubscriptionDto
                        {
                            Deadline = s.Deadline
                        })
                        .FirstOrDefault(),
                    FavouriteBooks = u.FavouriteBooks
                        .Select(b => new FavouriteBookDto
                        {
                            Id = b.Id,
                            Title = b.Title,
                            Author = $"{b.Author.FirstName} {b.Author.LastName}",
                            Cover = b.Cover,
                            ReleaseYear = b.ReleaseYear,
                            Rating = b.Reviews.Count == 0 ? 0 : b.Reviews
                                .Select(r => r.Value)
                                .Sum() / b.Reviews.Count,
                            Genres = b.Genres
                                .Select(g => g.Name)
                        })
                })
                .FirstOrDefaultAsync(u => string.Equals(u.Id, userId));

            ArgumentNullException.ThrowIfNull(user);

            return user;
        }
    }
}

