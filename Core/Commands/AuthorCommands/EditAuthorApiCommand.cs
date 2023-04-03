using Core.ApiModels.InputModels.Author;
using MediatR;

namespace Core.Commands.AuthorCommands
{
    public class EditAuthorApiCommand : IRequest<bool>
    {
        public EditAuthorApiCommand(string id, UpsertAuthorModel model)
        {
            Id = id;
            Model = model;
        }

        public UpsertAuthorModel Model { get; private set; }

        public string Id { get; private set; }
    }
}
