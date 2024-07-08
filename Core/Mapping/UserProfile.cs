using AutoMapper;
using Core.ApiModels.OutputModels.User;
using Core.Users.Queries.GetAllUsers;
using Core.Users.Queries.GetProfile;
using Domain.Entities;

namespace Core.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserProfileModel>();

            CreateMap<User, ListUserModel>();

            #region [Output Model]

            CreateMap<UserProfileModel, UserProfileModel>();

            CreateMap<ListUserModel, ListUserOutputModel>();

            #endregion
        }
    }
}
