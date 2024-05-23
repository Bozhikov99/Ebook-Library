using Core.ViewModels.Review;
using Domain.Entities;
using Infrastructure.Persistance;
using System.Linq.Expressions;

namespace Core.Reviews.Queries.GetUserReview
{
    public class GetUserReviewQuery : IRequest<UserReviewModel>
    {
        public string UserId { get; set; } = null!;

        public string BookId { get; set; } = null!;
    }

    public class GetUserReviewHandler : IRequestHandler<GetUserReviewQuery, UserReviewModel>
    {
        private readonly EbookDbContext context;

        public GetUserReviewHandler(EbookDbContext context)
        {
            this.context = context;
        }

        public async Task<UserReviewModel> Handle(GetUserReviewQuery request, CancellationToken cancellationToken)
        {
            string userId = request.UserId;
            string bookId = request.BookId;

            Expression<Func<Review, bool>> expression =
                 r => r.UserId == userId && r.BookId == bookId;

            UserReviewModel? userReview = await context.Reviews
                .Where(expression)
                .Select(r => new UserReviewModel
                {
                    Id = r.Id,
                    BookId = r.BookId,
                    Comment = r.Comment,
                    Value = r.Value
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (userReview is null)
            {
                throw new ArgumentException(nameof(Review), userId);
            }

            return userReview;
        }
    }
}

