using System;
using AutoMapper;
using Core.Helpers;
using Core.Queries.User;
using Core.ViewModels.Subscription;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Core.Handlers.UserHandlers
{
    public class GetActiveSubscriptionHandler : IRequestHandler<GetActiveSubscriptionQuery, ListSubscriptionModel>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly UserIdHelper helper;

        public GetActiveSubscriptionHandler(IRepository repository, IMapper mapper, UserIdHelper helper)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.helper = helper;
        }

        public async Task<ListSubscriptionModel> Handle(GetActiveSubscriptionQuery request, CancellationToken cancellationToken)
        {
            string userId = helper.GetUserId();
            Subscription subscription = await repository
                .All<Subscription>(s => s.UserId == userId && s.Deadline > DateTime.Now)
                .FirstOrDefaultAsync();

            ListSubscriptionModel model = mapper.Map<ListSubscriptionModel>(subscription);

            return model;
        }
    }
}

