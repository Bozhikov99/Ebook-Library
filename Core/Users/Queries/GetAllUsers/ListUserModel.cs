using Core.Common.Interfaces;

namespace Core.Users.Queries.GetAllUsers
{
    public class ListUserModel : IHypermediaResource
    {
        public string Id { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public IEnumerable<ILink> Links { get; set; } = new List<ILink>();
    }
}
