using AutoMapper;
using Common;
using Core.Services.Contracts;
using Core.ViewModels.Subscription;
using Infrastructure.Common;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public SubscriptionService(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task CreateSubscription(CreateSubscriptionModel model)
        {
            string userId = model.UserId;
            User user = await repository.All<User>(u => u.Id == userId)
                .Include(u => u.Subscriptions)
                .FirstAsync();

            ArgumentNullException.ThrowIfNull(user, ErrorMessageConstants.INVALID_USER);

            Subscription subscription = await repository.All<Subscription>(s => s.UserId == userId && s.Deadline > DateTime.Now)
                .FirstOrDefaultAsync();

            if (subscription == null)
            {
                subscription = mapper.Map<Subscription>(model);
                subscription.Deadline = DateTime.Now.AddDays(model.Days);
                subscription.Start = DateTime.Now;
            }
            else
            {
                user.Subscriptions.Remove(subscription);
                subscription.Deadline = subscription.Deadline.AddDays(model.Days);
                subscription.Price += model.Price;
            }

            user.Subscriptions.Add(subscription);

            repository.Update(user);
            await repository.SaveChangesAsync();
        }
    }
}
