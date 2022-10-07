using System;
using Core.ViewModels.Book;
using MediatR;

namespace Core.Queries.Book
{
    public class GetAllBooksQuery: IRequest<IEnumerable<ListBookModel>>
    {
        public GetAllBooksQuery(string search, string[] genres)
        {
            Search = search;
            Genres = genres;
        }

        public string Search { get; set; }

        public string[] Genres { get; set; }
    }
}

