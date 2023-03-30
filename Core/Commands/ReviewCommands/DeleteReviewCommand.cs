using System;
using MediatR;

namespace Core.Commands.ReviewCommands
{
    public class DeleteReviewCommand : IRequest
    {
        public DeleteReviewCommand(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}

