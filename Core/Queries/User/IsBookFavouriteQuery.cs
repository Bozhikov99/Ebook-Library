using System;
using MediatR;

namespace Core.Queries.User
{
    public class IsBookFavouriteQuery : IRequest<bool>
    {
        public IsBookFavouriteQuery(string bookId)
        {
            BookId = bookId;
        }

        public string BookId { get; private set; }
    }
}

