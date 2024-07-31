using Common;
using Core.Common.Services;
using Domain.Entities;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Identity;

namespace Core.Reviews.Commands.Delete
{
    public class DeleteReviewCommand : IRequest
    {
        public string Id { get; set; } = null!;
    }

    public class DeleteReviewHandler : IRequestHandler<DeleteReviewCommand>
    {
        private readonly UserManager<User> userManager;
        private readonly CurrentUserService userService;
        private readonly EbookDbContext context;

        public DeleteReviewHandler(EbookDbContext context, CurrentUserService userService, UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.userService = userService;
            this.context = context;
        }

        public async Task<Unit> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            string id = request.Id;

            Review? review = await context.Reviews
                .FirstOrDefaultAsync(r => string.Equals(r.Id, id));

            if (review is null)
            {
                throw new ArgumentNullException(nameof(Review), id);
            }

            string userId = userService.UserId!;

            User? user = await context.Users
                .FirstOrDefaultAsync(u => string.Equals(u.Id, userId));

            bool isAdmin = await userManager.IsInRoleAsync(user, RoleConstants.Administrator);
            bool isAuthor = string.Equals(review.UserId, userId);

            if (!isAdmin && !isAuthor)
            {
                throw new UnauthorizedAccessException();
            }

            context.Reviews
                .Remove(review);

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

