using Core.ViewModels.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Contracts
{
    public interface ISubscriptionService
    {
        Task CreateSubscription(CreateSubscriptionModel model);
    }
}
