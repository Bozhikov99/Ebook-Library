using System;
using Core.ViewModels.User;
using MediatR;

namespace Core.Queries.User
{
    public class GetAllUsersQuery : IRequest<IEnumerable<ListUserModel>>
    {
        public GetAllUsersQuery()
        {
        }
    }
}

