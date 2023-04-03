using Core.ApiModels.InputModels.Books;
using MediatR;

namespace Core.Queries.Book
{
    public class GetGuestBookDetailsApiQuery : IRequest<BookDetailsApiModel>
    {
        public GetGuestBookDetailsApiQuery(string bookId)
        {
            BookId = bookId;
        }

        public string BookId { get; private set; }
    }
}
