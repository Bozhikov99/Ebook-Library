using Core.ViewModels.Review;
using MediatR;

namespace Core.Queries.Review
{
    public class GetReviewQuery : IRequest<ListReviewModel>
    {
        public GetReviewQuery(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }
    }
}
