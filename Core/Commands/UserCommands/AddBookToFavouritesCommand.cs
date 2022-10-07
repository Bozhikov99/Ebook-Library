using System;
using MediatR;

namespace Core.Commands.UserCommands
{
    public class AddBookToFavouritesCommand : IRequest<bool>
    {
        public AddBookToFavouritesCommand(string bookId)
        {
            BookId = bookId;
        }

        public string BookId { get; set; }
    }
}

