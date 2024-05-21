using Core.ApiModels.OutputModels;

namespace Core.Books.Queries.GetBooks
{
    public class BookModel : OutputBaseModel
    {
        public string Title { get; set; } = null!;

        public byte[] Cover { get; set; } = null!;

        public int ReleaseYear { get; set; }

        public double Rating { get; set; }

        public IEnumerable<string> Genres { get; set; } = new List<string>();

        public string Author { get; set; } = null!;
    }
}
