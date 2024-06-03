using Common.MessageConstants;
using Domain.Entities;
using Infrastructure.Persistance;

namespace Core.Subscriptions.Commands.Create
{
    public class SubscribeCommand : IRequest
    {
        public decimal Price { get; set; }

        public int Days { get; set; }

        public string UserId { get; set; } = null!;
    }

    public class CreateSubscriptionCommandHandler : IRequestHandler<SubscribeCommand>
    {
        private readonly EbookDbContext context;

        public CreateSubscriptionCommandHandler(EbookDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(SubscribeCommand request, CancellationToken cancellationToken)
        {
            string userId = request.UserId;

            User user = await context.Users
                .Select(u => new User
                {
                    Id = u.Id,
                    Subscriptions = u.Subscriptions
                })
                .FirstAsync(u => string.Equals(u.Id, userId));

            ArgumentNullException.ThrowIfNull(user, ErrorMessageConstants.INVALID_USER);

            Subscription? subscription = await context.Subscriptions
                .FirstOrDefaultAsync(s => s.UserId == userId && s.Deadline > DateTime.Now, cancellationToken);

            if (subscription is null)
            {
                subscription = new Subscription
                {
                    UserId = userId,
                    Price = request.Price,
                    Deadline = DateTime.Now.AddDays(request.Days),
                    Start = DateTime.Now
                };
            }
            else
            {
                user.Subscriptions.Remove(subscription);
                subscription.Deadline = subscription.Deadline
                    .AddDays(request.Days);

                subscription.Price += request.Price;
            }

            user.Subscriptions
                .Add(subscription);

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

