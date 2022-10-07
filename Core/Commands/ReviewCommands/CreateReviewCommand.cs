using System;
using Core.ViewModels.Review;
using MediatR;

namespace Core.Commands.ReviewCommands
{
    public class CreateReviewCommand: IRequest<bool>
    {
        public CreateReviewCommand(CreateReviewModel model)
        {
            Model = model;
        }

        public CreateReviewModel Model { get; set; }
    }
}

