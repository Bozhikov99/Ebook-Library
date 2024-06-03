using Core.Common.Interfaces;

namespace Core.Users.Queries.GetUserProfile
{
    public class UserProfileModel : IHypermediaResource
    {
        public string Id { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public DateTime RegisterDate { get; set; }

        public ActiveSubscriptionDto? Subscription { get; set; }

        public IEnumerable<FavouriteBookDto> FavouriteBooks { get; set; } = new List<FavouriteBookDto>();

        public IEnumerable<ILink> Links { get; set; } = new List<ILink>();
    }
}
