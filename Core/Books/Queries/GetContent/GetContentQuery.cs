using Common.MessageConstants;
using Core.Helpers;
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
        private readonly UserIdHelper helper;
        private readonly EbookDbContext context;

        public GetContentHandler(EbookDbContext context, UserIdHelper helper)
        {
            this.helper = helper;
            this.context = context;
        }

        public async Task<byte[]> Handle(GetContentQuery request, CancellationToken cancellationToken)
        {
            string userId = helper.GetUserId();

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

