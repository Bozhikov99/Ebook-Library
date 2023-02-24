using Core.ApiModels.Author;
using Core.ViewModels.Author;
using MediatR;

namespace Core.Commands.AuthorCommands
{
    public class CreateAuthorApiCommand : IRequest<ListAuthorModel>
    {
        public CreateAuthorApiCommand(UpsertAuthorModel model)
        {
            Model = model;
        }

        public UpsertAuthorModel Model { get; private set; }
    }
}
