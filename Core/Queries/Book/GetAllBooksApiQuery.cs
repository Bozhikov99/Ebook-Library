using Core.ApiModels.Books;
using MediatR;

namespace Core.Queries.Book
{
    public class GetAllBooksApiQuery : IRequest<BooksBrowsingModel>
    {
        public GetAllBooksApiQuery(string search, string[] genres)
        {
            Genres = genres;
            Search = search;
        }

        public string Search { get; private set; }

        public string[] Genres { get; private set; }
    }
}
