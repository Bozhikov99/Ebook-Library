using Common.MessageConstants;
using Core.Reviews.Common;
using Infrastructure.Persistance;

namespace Core.Reviews.Queries.GetReviewDetails
{
    public class GetReviewDetailsQuery : IRequest<ReviewModel>
    {
        public string Id { get; set; } = null!;
    }

    public class GetReviewHandler : IRequestHandler<GetReviewDetailsQuery, ReviewModel>
    {
        private readonly EbookDbContext context;

        public GetReviewHandler(EbookDbContext context)
        {
            this.context = context;
        }

        public async Task<ReviewModel> Handle(GetReviewDetailsQuery request, CancellationToken cancellationToken)
        {
            string id = request.Id;

            ReviewModel? model = await context.Reviews
                .Select(r => new ReviewModel
                {
                    Id = r.Id,
                    BookId = r.BookId,
                    Comment = r.Comment,
                    Value = r.Value,
                    UserName = r.User.UserName
                })
                .FirstOrDefaultAsync(r => string.Equals(r.Id, id), cancellationToken);

            if (model is null)
            {
                throw new ArgumentNullException(ErrorMessageConstants.REVIEW_NOT_FOUND);
            }

            return model;
        }
    }
}
