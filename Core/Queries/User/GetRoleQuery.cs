using Core.ApiModels.User;
using MediatR;

namespace Core.Queries.User
{
    public class GetRoleQuery : IRequest<RoleInfoModel>
    {
        public GetRoleQuery(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }
    }
}
