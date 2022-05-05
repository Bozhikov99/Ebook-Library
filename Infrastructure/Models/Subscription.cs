using Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
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

        public SubscriptionType Type { get; set; }

        public DateTime Deadline { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public virtual User User { get; set; }
    }
}
