using Core.Books.Queries.Details;
using MediatR;

namespace Core.Queries.Book
{
    public class GetGuestBookDetailsApiQuery : IRequest<BookDetailsOutputModel>
    {
        public GetGuestBookDetailsApiQuery(string bookId)
        {
            BookId = bookId;
        }

        public string BookId { get; private set; }
    }
}
