using Common.MessageConstants;
using Core.Common.Services;
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
        private readonly CurrentUserService userService;

        public RemoveBookFromFavouritesCommandHandler(EbookDbContext context, CurrentUserService userService)
        {
            this.context = context;
            this.userService = userService;
        }

        public async Task<Unit> Handle(RemoveBookFromFavouritesCommand request, CancellationToken cancellationToken)
        {
            string bookId = request.BookId;
            string userId = userService.UserId!;

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

