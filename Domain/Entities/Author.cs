using Common.ValidationConstants;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Author
    {
        public Author()
        {
            Id = Guid.NewGuid()
                .ToString();

            Books = new HashSet<Book>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(AuthorConstants.AUTHOR_NAME_MAXLENGTH)]
        public string FirstName { get; set; }

        [MaxLength(AuthorConstants.AUTHOR_NAME_MAXLENGTH)]
        public string LastName { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
