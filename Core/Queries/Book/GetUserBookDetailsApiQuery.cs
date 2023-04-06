using Core.ApiModels.InputModels.Books;
using Core.ApiModels.OutputModels.Book;
using MediatR;

namespace Core.Queries.Book
{
    public class GetUserBookDetailsApiQuery : IRequest<BookDetailsOutputModel>
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
