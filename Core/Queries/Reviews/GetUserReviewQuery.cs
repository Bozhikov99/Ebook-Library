using System;
using Core.ViewModels.Review;
using MediatR;

namespace Core.Queries.Review
{
    public class GetUserReviewQuery : IRequest<UserReviewModel>
    {
        public GetUserReviewQuery(string userId, string bookId)
        {
            BookId = bookId;
            UserId = userId;
        }

        public string UserId { get; set; }
        public string BookId { get; set; }
    }
}

