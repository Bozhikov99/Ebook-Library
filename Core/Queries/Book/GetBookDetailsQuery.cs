using Core.ViewModels.Book;
using MediatR;

namespace Core.Queries.Book
{
    public class GetBookDetailsQuery : IRequest<BookDetailsModel>
    {
        public GetBookDetailsQuery(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}

