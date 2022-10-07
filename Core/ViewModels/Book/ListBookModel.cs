namespace Core.ViewModels.Book
{
    public class ListBookModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public byte[] Cover { get; set; }

        public int ReleaseYear { get; set; }

        public double Rating { get; set; }

        public string[] Genres { get; set; }

        public string Author { get; set; }
    }
}
