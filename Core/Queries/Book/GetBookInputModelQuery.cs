using Core.ApiModels.InputModels.Books;
using MediatR;

namespace Core.Queries.Book
{
    public class GetBookInputModelQuery : IRequest<BookInputModel>
    {
        public GetBookInputModelQuery(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }
    }
}
