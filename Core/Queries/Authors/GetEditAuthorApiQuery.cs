using Core.ApiModels.InputModels.Author;
using MediatR;

namespace Core.Queries.Author
{
    public class GetEditAuthorApiQuery : IRequest<UpsertAuthorModel>
    {
        public GetEditAuthorApiQuery(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }
    }
}
