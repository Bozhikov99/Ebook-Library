using System;
using Core.ViewModels.Genre;
using MediatR;

namespace Core.Commands.GenreCommands
{
    public class EditGenreCommand : IRequest<bool>
    {
        public EditGenreCommand(EditGenreModel model)
        {
            Model = model;
        }

        public EditGenreModel Model { get; set; }
    }
}

