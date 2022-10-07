using System;
using Core.ViewModels.Subscription;
using MediatR;

namespace Core.Commands.SubscriptionCommands
{
    public class CreateSubscriptionCommand : IRequest<bool>
    {
        public CreateSubscriptionCommand(CreateSubscriptionModel model)
        {
            Model = model;
        }

        public CreateSubscriptionModel Model { get; private set; }
    }
}

