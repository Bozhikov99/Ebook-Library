using AutoMapper;
using Core.ViewModels.Subscription;
using Infrastructure.Models;

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
