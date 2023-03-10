﻿using AutoMapper;
using Core.ApiModels.User;
using Core.ViewModels.User;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

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

            CreateMap<IdentityRole, RoleInfoModel>();
        }
    }
}
