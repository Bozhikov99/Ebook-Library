using Common.ValidationConstants;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class User : IdentityUser
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

        public DateTime RegisterDate { get; set; }

        public virtual ICollection<Subscription> Subscriptions { get; set; }

        public virtual ICollection<Book> FavouriteBooks { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}
