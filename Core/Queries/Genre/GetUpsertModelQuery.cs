using Core.ApiModels.Genre;
using MediatR;

namespace Core.Queries.Genre
{
    public class GetUpsertModelQuery : IRequest<UpsertGenreModel>
    {
        public GetUpsertModelQuery(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }
    }
}
