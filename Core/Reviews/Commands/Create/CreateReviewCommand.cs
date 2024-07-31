using Core.Reviews.Common;
using Domain.Entities;
using Infrastructure.Persistance;

namespace Core.Reviews.Commands.Create
{
    public class CreateReviewCommand : IRequest<BaseReviewModel>
    {
        public string BookId { get; set; } = null!;

        public double Value { get; set; }

        public string? Comment { get; set; }

        public string UserId { get; set; } = null!;
    }

    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, BaseReviewModel>
    {
        private readonly EbookDbContext context;

        public CreateReviewCommandHandler(EbookDbContext context)
        {
            this.context = context;
        }

        public async Task<BaseReviewModel> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            string userId = request.UserId;
            string bookId = request.BookId;

            User? user = await context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
            {
                throw new ArgumentException(nameof(User), userId);
            }

            Review? existingReview = await context.Reviews
                .FirstOrDefaultAsync(r => r.UserId == userId && r.BookId == bookId);

            if (existingReview != null)
            {
                context.Reviews
                    .Remove(existingReview);
            }

            Review review = new Review
            {
                BookId = bookId,
                UserId = userId,
                Value = request.Value,
                Comment = request.Comment
            };

            await context.Reviews
                .AddAsync(review);

            await context.SaveChangesAsync(cancellationToken);

            ReviewModel model = new ReviewModel
            {
                Id = review.Id,
                Value = review.Value,
                Comment = review.Comment,
                UserName = user.UserName,
                BookId = bookId
            };

            return model;
        }
    }
}
