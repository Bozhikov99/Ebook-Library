using Core.ApiModels.OutputModels.User;
using MediatR;

namespace Core.Queries.User
{
    public class GetProfileQuery : IRequest<UserProfileOutputModel>
    {
        public GetProfileQuery(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }
    }
}
