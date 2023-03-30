using Core.ApiModels.Books;
using MediatR;

namespace Core.Queries.Book
{
    public class GetUserBookDetailsApiQuery : IRequest<BookDetailsApiModel>
    {
        public GetUserBookDetailsApiQuery(string bookId, string userId)
        {
            BookId = bookId;
            UserId = userId;
        }

        public string BookId { get; private set; }

        public string UserId { get; private set; }
    }
}
