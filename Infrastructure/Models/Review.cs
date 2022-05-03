using Common.ValidationConstants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class Review
    {
        public Review()
        {
            Id = Guid.NewGuid()
                .ToString();
        }

        [Key]
        public string Id { get; set; }

        [Range(ReviewConstants.VALUE_MIN, ReviewConstants.VALUE_MAX)]
        public double Value { get; set; }

        [MaxLength(ReviewConstants.COMMENT_MAXLENGTH)]
        public string Comment { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        [ForeignKey(nameof(Book))]
        public string BookId { get; set; }

        public virtual Book Book { get; set; }
    }
}
