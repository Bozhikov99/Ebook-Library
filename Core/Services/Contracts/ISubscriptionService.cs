using Core.ViewModels.Subscription;

namespace Core.Services.Contracts
{
    public interface ISubscriptionService
    {
        Task CreateSubscription(CreateSubscriptionModel model);
    }
}
