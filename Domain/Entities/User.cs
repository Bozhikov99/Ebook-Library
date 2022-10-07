using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
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

        public DateTime RegisterDate { get; set; }

        public virtual ICollection<Subscription> Subscriptions { get; set; }

        public virtual ICollection<Book> FavouriteBooks { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}
