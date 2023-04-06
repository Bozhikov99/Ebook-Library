using Core.ApiModels.ResponseModels;
using MediatR;

namespace Core.Queries.Book
{
    public class GetAllBooksApiQuery : IRequest<BookBrowsingModel>
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
