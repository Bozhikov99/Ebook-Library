using System;
using AutoMapper;
using Common.MessageConstants;
using Core.Commands.SubscriptionCommands;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Handlers.SubscriptionHandlers
{
    public class CreateSubscriptionHandler : IRequestHandler<CreateSubscriptionCommand, bool>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;


        public CreateSubscriptionHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<bool> Handle(
            CreateSubscriptionCommand request
            , CancellationToken cancellationToken)
        {
            bool isSuccessful = false;

            string userId = request.Model.UserId;

            User user = await repository.All<User>(u => u.Id == userId)
                .Include(u => u.Subscriptions)
                .FirstAsync();

            ArgumentNullException.ThrowIfNull(user, ErrorMessageConstants.INVALID_USER);

            Subscription subscription = await repository.All<Subscription>(s => s.UserId == userId && s.Deadline > DateTime.Now)
                .FirstOrDefaultAsync();

            if (subscription == null)
            {
                subscription = mapper.Map<Subscription>(request.Model);
                subscription.Deadline = DateTime.Now.AddDays(request.Model.Days);
                subscription.Start = DateTime.Now;
            }
            else
            {
                user.Subscriptions.Remove(subscription);
                subscription.Deadline = subscription.Deadline.AddDays(request.Model.Days);
                subscription.Price += request.Model.Price;
            }

            user.Subscriptions.Add(subscription);

            repository.Update(user);
            await repository.SaveChangesAsync();

            isSuccessful = true;

            return isSuccessful;
        }
    }
}

