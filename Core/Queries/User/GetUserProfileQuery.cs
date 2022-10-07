using System;
using Core.ViewModels.User;
using MediatR;

namespace Core.Queries.User
{
    public class GetUserProfileQuery: IRequest<UserProfileModel>
    {
        public GetUserProfileQuery()
        {
        }
    }
}

