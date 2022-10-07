using System;
using Core.ViewModels.Book;
using MediatR;

namespace Core.Commands.BookCommands
{
    public class CreateBookCommand : IRequest<bool>
    {
        public CreateBookCommand(CreateBookModel model)
        {
            Model = model;
        }

        public CreateBookModel Model { get; private set; }
    }
}

