using System;
using MediatR;

namespace Core.Queries.User
{
    public class IsUserAdminQuery: IRequest<bool>
    {
        public IsUserAdminQuery()
        {
        }
    }
}

