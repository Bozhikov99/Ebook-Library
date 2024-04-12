using System;
using Core.ViewModels.Review;
using MediatR;

namespace Core.Queries.Review
{
    public class GetAllReviewsQuery : IRequest<IEnumerable<ListReviewModel>>
    {
        public GetAllReviewsQuery(string bookId, string userId)
        {
            BookId = bookId;
            UserId = userId;
        }

        public string BookId { get; private set; }
        public string UserId { get; private set; }
    }
}

