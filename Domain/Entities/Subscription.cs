using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Subscription
    {
        public Subscription()
        {
            Id = Guid.NewGuid()
                .ToString();
        }

        [Key]
        public string Id { get; set; }

        public decimal Price { get; set; }

        public DateTime Start { get; set; }

        public DateTime Deadline { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}
