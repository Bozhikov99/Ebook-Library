using AutoMapper;
using Core.ApiModels.OutputModels.User;
using Core.ViewModels.User;
using Domain.Entities;

namespace Core.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterUserModel, User>();

            CreateMap<LoginUserModel, User>();

            CreateMap<RegisterUserModel, LoginUserModel>();

            CreateMap<User, UserProfileModel>();

            CreateMap<User, ListUserModel>();

            #region [Output Model]

            CreateMap<UserProfileModel, UserProfileModel>();

            CreateMap<ListUserModel, ListUserOutputModel>();

            #endregion
        }
    }
}
