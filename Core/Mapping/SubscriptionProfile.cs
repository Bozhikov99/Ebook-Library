using AutoMapper;
using Core.ViewModels.Subscription;
using Domain.Entities;

namespace Core.Mapping
{
    public class SubscriptionProfile : Profile
    {
        public SubscriptionProfile()
        {
            CreateMap<CreateSubscriptionModel, Subscription>();

            CreateMap<Subscription, ListSubscriptionModel>();
        }
    }
}
