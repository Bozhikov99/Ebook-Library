using Core.ApiModels.InputModels.Genre;
using Core.ViewModels.Genre;
using MediatR;

namespace Core.Commands.GenreCommands
{
    public class EditGenreApiCommand : IRequest<bool>
    {
        public EditGenreApiCommand(string id, UpsertGenreModel model)
        {
            Id = id;
            Model = model;
        }

        public UpsertGenreModel Model { get; private set; }

        public string Id { get; private set; }
    }
}
