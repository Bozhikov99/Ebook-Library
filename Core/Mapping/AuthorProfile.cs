using AutoMapper;
using Core.ApiModels.InputModels.Author;
using Core.ApiModels.OutputModels.Author;
using Core.ViewModels.Author;
using Domain.Entities;

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

            CreateMap<Author, UpsertAuthorModel>()
                .ReverseMap();

            #region [Output Models]

            CreateMap<ListAuthorModel, ListAuthorOutputModel>();

            #endregion
        }
    }
}
