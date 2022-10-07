using System;
using Core.ViewModels.Genre;
using MediatR;

namespace Core.Commands.GenreCommands
{
    public class CreateGenreCommand: IRequest<string>
    {
        public CreateGenreCommand(CreateGenreModel model)
        {
            Model = model;
        }

        public CreateGenreModel Model { get; private set; }
    }
}

