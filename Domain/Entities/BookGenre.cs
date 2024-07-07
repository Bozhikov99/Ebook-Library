namespace Domain.Entities
{
    public class BookGenre
    {
        public string BookId { get; set; } = null!;

        public Book Book { get; set; } = null!;

        public string GenreId { get; set; } = null!;

        public Genre Genre { get; set; } = null!;
    }
}
