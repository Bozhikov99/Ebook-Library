using MediatR;

namespace Core.Commands.ReviewCommands
{
    public class DeleteReviewApiCommand : IRequest
    {
        public DeleteReviewApiCommand(string id, string userId)
        {
            Id = id;
            UserId = userId;
        }

        public string Id { get; private set; }
        
        public string UserId { get; private set; }
    }
}
