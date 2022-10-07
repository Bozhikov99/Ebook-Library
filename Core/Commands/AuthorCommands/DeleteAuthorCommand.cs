using System;
using MediatR;

namespace Core.Commands.AuthorCommands
{
    public class DeleteAuthorCommand: IRequest<bool>
    {
        public DeleteAuthorCommand(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }
    }
}

