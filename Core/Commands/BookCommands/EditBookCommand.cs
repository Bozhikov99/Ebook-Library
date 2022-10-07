using System;
using Core.ViewModels.Book;
using MediatR;

namespace Core.Commands.BookCommands
{
    public class EditBookCommand: IRequest<bool>
    {
        public EditBookCommand(EditBookModel model)
        {
            Model = model;
        }

        public EditBookModel Model { get; set; }
    }
}

