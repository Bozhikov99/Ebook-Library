using Common.MessageConstants;
using Core.Common.Services;
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
        private readonly CurrentUserService userService;
        private readonly EbookDbContext context;

        public AddBookToFavouritesCommandHandler(EbookDbContext context, CurrentUserService userService)
        {
            this.context = context;
            this.userService = userService;
        }

        public async Task<Unit> Handle(AddBookToFavouritesCommand request, CancellationToken cancellationToken)
        {
            string bookId = request.BookId;
            string userId = userService.UserId!;

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

