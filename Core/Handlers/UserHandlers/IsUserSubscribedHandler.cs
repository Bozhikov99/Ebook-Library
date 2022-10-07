using System;
using AutoMapper;
using Core.Helpers;
using Core.Queries.User;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Handlers.UserHandlers
{
    public class IsUserSubscribedHandler: IRequestHandler<IsUserSubscribedQuery, bool>
    {
        private readonly IRepository repository;
        private readonly UserIdHelper helper;

        public IsUserSubscribedHandler(IRepository repository, UserIdHelper helper)
        {
            this.repository = repository;
            this.helper = helper;
        }

        public async Task<bool> Handle(
            IsUserSubscribedQuery request,
            CancellationToken cancellationToken)
        {
            string userId = helper.GetUserId();
            Subscription subscription = await repository.All<Subscription>(s => s.UserId == userId)
                .FirstOrDefaultAsync(s => s.Deadline > DateTime.Now);

            return subscription != null;
        }
    }
}

