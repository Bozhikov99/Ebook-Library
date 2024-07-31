using Core.Common.Interfaces;

namespace Core.Books.Queries.GetBooks
{
    public class BookModel : IHypermediaResource
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public byte[] Cover { get; set; } = null!;

        public int ReleaseYear { get; set; }

        public double Rating { get; set; }

        public IEnumerable<string> Genres { get; set; } = new List<string>();

        public string Author { get; set; } = null!;

        public IEnumerable<ILink> Links { get; set; } = new List<ILink>();
    }
}
