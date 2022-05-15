using AutoMapper;
using Core.ViewModels.Subscription;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
