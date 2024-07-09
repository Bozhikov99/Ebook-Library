using Common.MessageConstants;
using Core.Helpers;
using Domain.Entities;
using Infrastructure.Persistance;

namespace Core.Users.Commands.AddBookToFavourites
{
    public class AddBookToFavouritesCommand : IRequest
    {
        public string BookId { get; set; } = null!;
    }

    public class AddBookToFavouritesCommandHandler : IRequestHandler<AddBookToFavouritesCommand>
    {
        private readonly UserIdHelper userIdHelper;
        private readonly EbookDbContext context;

        public AddBookToFavouritesCommandHandler(EbookDbContext context, UserIdHelper userIdHelper)
        {
            this.context = context;
            this.userIdHelper = userIdHelper;
        }

        public async Task<Unit> Handle(AddBookToFavouritesCommand request, CancellationToken cancellationToken)
        {
            string bookId = request.BookId;
            string userId = userIdHelper.GetUserId();

            User? user = await context.Users
                .FirstOrDefaultAsync(u => string.Equals(u.Id, userId), cancellationToken);

            ArgumentNullException.ThrowIfNull(user, ErrorMessageConstants.INVALID_USER);

            Book? book = await context.Books
                .FirstOrDefaultAsync(b => string.Equals(b.Id, bookId), cancellationToken);

            ArgumentNullException.ThrowIfNull(book);

            BookUser bookUser = new BookUser
            {
                Book = book,
                User = user
            };

            user.FavouriteBooks
                .Add(bookUser);

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

