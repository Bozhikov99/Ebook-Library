using System;
using MediatR;

namespace Core.Commands.UserCommands
{
    public class RemoveBookFromFavouritesCommand : IRequest<bool>
    {
        public RemoveBookFromFavouritesCommand(string bookId)
        {
            BookId = bookId;
        }

        public string BookId { get; set; }
    }
}

