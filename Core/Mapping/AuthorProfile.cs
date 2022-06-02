using AutoMapper;
using Core.ViewModels.Author;
using Infrastructure.Models;

namespace Core.Mapping
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<CreateAuthorModel, Author>();

            CreateMap<EditAuthorModel, Author>()
                .ReverseMap();

            CreateMap<Author, ListAuthorModel>();
        }
    }
}
