using System;
using Core.ViewModels.Subscription;
using MediatR;

namespace Core.Queries.User
{
    public class GetActiveSubscriptionQuery : IRequest<ListSubscriptionModel>
    {
        public GetActiveSubscriptionQuery()
        {
        }
    }
}

