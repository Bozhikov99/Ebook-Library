using Common.ValidationConstants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
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
