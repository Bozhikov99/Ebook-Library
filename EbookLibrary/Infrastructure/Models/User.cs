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
    public class User
    {
        public User()
        {
            Id = Guid.NewGuid()
                .ToString();

            FavouriteBooks = new HashSet<Book>();
            Reviews = new HashSet<Review>();
            Subscriptions = new HashSet<Subscription>();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(UserConstants.USERNAME_MAXLENGTH)]
        public string Username { get; set; }

        [Required]
        [MaxLength(UserConstants.EMAIL_MAXLENGTH)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public DateTime RegisterDate { get; set; }

        public virtual ICollection<Subscription> Subscriptions { get; set; }

        public virtual ICollection<Book> FavouriteBooks { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}
