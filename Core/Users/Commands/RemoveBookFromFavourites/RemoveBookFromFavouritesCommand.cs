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
                .Select(u => new User
                {
                    Id = u.Id,
                    FavouriteBooks = u.FavouriteBooks
                })
                .FirstOrDefaultAsync(u => string.Equals(u.Id, userId), cancellationToken);

            ArgumentNullException.ThrowIfNull(user, ErrorMessageConstants.INVALID_USER);

            BookUser? bookUser = user.FavouriteBooks
                .FirstOrDefault(bu => string.Equals(bu.BookId, bookId));

            ArgumentNullException.ThrowIfNull(bookUser);

            context.BookUsers
                .Remove(bookUser);

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

