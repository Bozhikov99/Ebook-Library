using System;
using Core.ViewModels.Author;
using MediatR;

namespace Core.Commands.AuthorCommands
{
    public class CreateAuthorCommand: IRequest<bool>
    {
        public CreateAuthorCommand(CreateAuthorModel model)
        {
            Model = model;
        }

        public CreateAuthorModel Model { get; set; }
    }
}

