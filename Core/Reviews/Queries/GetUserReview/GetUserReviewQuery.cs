using Core.ApiModels.OutputModels.Review;
using Domain.Entities;
using Infrastructure.Persistance;
using System.Linq.Expressions;

namespace Core.Reviews.Queries.GetUserReview
{
    public class GetUserReviewQuery : IRequest<UserReviewOutputModel>
    {
        public string UserId { get; set; } = null!;

        public string BookId { get; set; } = null!;
    }

    public class GetUserReviewHandler : IRequestHandler<GetUserReviewQuery, UserReviewOutputModel>
    {
        private readonly EbookDbContext context;

        public GetUserReviewHandler(EbookDbContext context)
        {
            this.context = context;
        }

        public async Task<UserReviewOutputModel> Handle(GetUserReviewQuery request, CancellationToken cancellationToken)
        {
            string userId = request.UserId;
            string bookId = request.BookId;

            Expression<Func<Review, bool>> expression =
                 r => r.UserId == userId && r.BookId == bookId;

            UserReviewOutputModel? userReview = await context.Reviews
                .Where(expression)
                .Select(r => new UserReviewOutputModel
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

