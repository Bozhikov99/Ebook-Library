using System;
using MediatR;

namespace Core.Commands.BookCommands
{
    public class DeleteBookCommand : IRequest<bool>
    {
        public DeleteBookCommand(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}

