namespace Core.ViewModels.Book
{
    public class BookDetailsModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public byte[] Cover { get; set; }

        public int ReleaseYear { get; set; }

        public int Pages { get; set; }

        public string Author { get; set; }

        public string[] Genres { get; set; }

        public double Rating { get; set; }
    }
}
