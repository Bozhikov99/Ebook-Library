using Core.Common.Interfaces;

namespace Core.Genres.Queries.Common
{
    public class GenreModel : IHypermediaResource
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public IEnumerable<ILink> Links { get; set; } = new List<ILink>();
    }
}
