using Core.Common.Interfaces;

namespace Core.Authors.Queries.Common
{
    public class AuthorModel : IHypermediaResource
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Id { get; set; } = null!;

        public IEnumerable<ILink> Links { get; set; } = new List<ILink>();
    }
}
