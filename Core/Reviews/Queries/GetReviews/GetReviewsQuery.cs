using Core.Reviews.Common;
using Infrastructure.Persistance;

namespace Core.Reviews.Queries.GetReviews
{
    public class GetReviewsQuery : IRequest<IEnumerable<BaseReviewModel>>
    {
        public string BookId { get; set; } = null!;

        public string UserId { get; set; } = null!;
    }

    public class GetReviewsHandler : IRequestHandler<GetReviewsQuery, IEnumerable<BaseReviewModel>>
    {
        private readonly EbookDbContext context;

        public GetReviewsHandler(EbookDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<BaseReviewModel>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
        {
            string userId = request.UserId;
            string bookId = request.BookId;

            IEnumerable<ReviewModel> reviews = await context.Reviews
                .Where(r => !string.Equals(r.UserId, userId) && string.Equals(r.BookId, bookId))
                .Select(r => new ReviewModel
                {
                    Id = r.Id,
                    BookId = r.BookId,
                    Comment = r.Comment,
                    Value = r.Value,
                    UserName = r.User.UserName
                })
                .ToArrayAsync();

            return reviews;
        }
    }
}

