using System;
using Core.ViewModels.Book;
using MediatR;

namespace Core.Queries.User
{
    public class GetFavouriteBooksQuery : IRequest<IEnumerable<ListBookModel>>
    {
        public GetFavouriteBooksQuery()
        {
        }
    }
}

