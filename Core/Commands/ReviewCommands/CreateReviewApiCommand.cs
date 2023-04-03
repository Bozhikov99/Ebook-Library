using Core.ApiModels.InputModels.Review;
using Core.ViewModels.Review;
using MediatR;

namespace Core.Commands.ReviewCommands
{
    public class CreateReviewApiCommand : IRequest<ListReviewModel>
    {
        public CreateReviewApiCommand(string bookId, ReviewInputModel model)
        {
            BookId = bookId;
            Model = model;
        }

        public string BookId { get; set; }

        public ReviewInputModel Model { get; set; }
    }
}
