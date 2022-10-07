using System;
using MediatR;

namespace Core.Commands.GenreCommands
{
    public class DeleteGenreCommand : IRequest<bool>
    {
        public DeleteGenreCommand(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }
    }
}

