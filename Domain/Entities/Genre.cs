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

            BookGenres = new HashSet<BookGenre>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(GenreConstants.NAME_MAX_LENGTH)]
        public string Name { get; set; }

        public virtual ICollection<BookGenre> BookGenres { get; set; }
    }
}
