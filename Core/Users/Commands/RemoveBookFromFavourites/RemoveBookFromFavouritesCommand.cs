using Common.MessageConstants;
using Core.Helpers;
using Domain.Entities;
using Infrastructure.Persistance;

namespace Core.Users.Commands.RemoveBookFromFavourites
{
    public class RemoveBookFromFavouritesCommand : IRequest
    {
        public string BookId { get; set; } = null!;
    }

    public class RemoveBookFromFavouritesCommandHandler : IRequestHandler<RemoveBookFromFavouritesCommand>
    {
        private readonly EbookDbContext context;
        private readonly UserIdHelper userIdHelper;

        public RemoveBookFromFavouritesCommandHandler(EbookDbContext context, UserIdHelper userIdHelper)
        {
            this.context = context;
            this.userIdHelper = userIdHelper;
        }

        public async Task<Unit> Handle(RemoveBookFromFavouritesCommand request, CancellationToken cancellationToken)
        {
            string bookId = request.BookId;
            string userId = userIdHelper.GetUserId();

            User? user = await context.Users
                .FirstOrDefaultAsync(u => string.Equals(u.Id, userId), cancellationToken);

            ArgumentNullException.ThrowIfNull(user, ErrorMessageConstants.INVALID_USER);

            Book? book = await context.Books
                .FirstOrDefaultAsync(b => string.Equals(b.Id, bookId), cancellationToken);

            ArgumentNullException.ThrowIfNull(book);

            user.FavouriteBooks
                .Remove(book);

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

