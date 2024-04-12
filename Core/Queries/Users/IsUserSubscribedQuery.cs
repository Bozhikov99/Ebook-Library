using System;
using MediatR;

namespace Core.Queries.User
{
    public class IsUserSubscribedQuery : IRequest<bool>
    {
        public IsUserSubscribedQuery()
        {
        }
    }
}

