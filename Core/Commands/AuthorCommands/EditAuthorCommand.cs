using System;
using Core.ViewModels.Author;
using MediatR;

namespace Core.Commands.AuthorCommands
{
    public class EditAuthorCommand : IRequest<bool>
    {
        public EditAuthorCommand(EditAuthorModel model)
        {
            Model = model;
        }

        public EditAuthorModel Model { get; set; }
    }
}

