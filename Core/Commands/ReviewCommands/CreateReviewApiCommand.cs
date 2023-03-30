using Core.ApiModels.Review;
using Core.ViewModels.Review;
using MediatR;

namespace Core.Commands.ReviewCommands
{
    public class CreateReviewApiCommand : IRequest<ListReviewModel>
    {
        public CreateReviewApiCommand(string bookId, CreateReviewApiModel model)
        {
            BookId = bookId;
            Model = model;
        }

        public string BookId { get; set; }

        public CreateReviewApiModel Model { get; set; }
    }
}
