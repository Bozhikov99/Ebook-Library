namespace Core.Users.Queries.GetUserProfile
{
    public class FavouriteBookDto
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public byte[] Cover { get; set; }

        public int ReleaseYear { get; set; }

        public double Rating { get; set; }

        public IEnumerable<string> Genres { get; set; } = new List<string>();

        public string Author { get; set; }
    }
}
