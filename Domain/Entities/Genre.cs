using Common.ValidationConstants;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Genre
    {
        public Genre()
        {
            Id = Guid.NewGuid()
                .ToString();

            Books = new HashSet<Book>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(GenreConstants.NAME_MAX_LENGTH)]
        public string Name { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
