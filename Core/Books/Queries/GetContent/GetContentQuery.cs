using Common.MessageConstants;
using Core.Common.Services;
using Domain.Entities;
using Infrastructure.Persistance;

namespace Core.Books.Queries.GetContent
{
    public class GetContentQuery : IRequest<byte[]>
    {
        public string BookId { get; set; } = null!;
    }

    public class GetContentHandler : IRequestHandler<GetContentQuery, byte[]>
    {
        private readonly CurrentUserService userService;
        private readonly EbookDbContext context;

        public GetContentHandler(EbookDbContext context, CurrentUserService helper)
        {
            this.userService = helper;
            this.context = context;
        }

        public async Task<byte[]> Handle(GetContentQuery request, CancellationToken cancellationToken)
        {
            string userId = userService.UserId!;

            bool isSubscribed = await context.Subscriptions
                .AnyAsync(s => string.Equals(s.UserId, userId) && s.Deadline > DateTime.Now);

            if (!isSubscribed)
            {
                throw new InvalidOperationException("Please subscribe");
            }

            string bookId = request.BookId;

            Book? book = await context.Books
                .FirstOrDefaultAsync(b => string.Equals(b.Id, bookId));

            ArgumentNullException.ThrowIfNull(book, ErrorMessageConstants.BOOK_DOES_NOT_EXIST);

            return book.Content;
        }
    }
}

