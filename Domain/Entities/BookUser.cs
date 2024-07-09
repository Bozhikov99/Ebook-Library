namespace Domain.Entities
{
    public class BookUser
    {
        public string BookId { get; set; } = null!;
        
        public Book Book { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public User User { get; set; } = null!;
    }
}
