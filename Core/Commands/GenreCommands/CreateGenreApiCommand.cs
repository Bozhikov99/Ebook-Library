using Core.ApiModels.InputModels.Genre;
using Core.ViewModels.Genre;
using MediatR;

namespace Core.Commands.GenreCommands
{
    public class CreateGenreApiCommand : IRequest<ListGenreModel>
    {
        public CreateGenreApiCommand(UpsertGenreModel model)
        {
            Model = model;
        }

        public UpsertGenreModel Model { get; private set; }
    }
}
