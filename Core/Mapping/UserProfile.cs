using AutoMapper;
using Core.ViewModels.User;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterUserModel, User>();

            CreateMap<LoginUserModel, User>();

            CreateMap<RegisterUserModel, LoginUserModel>();
        }
    }
}
