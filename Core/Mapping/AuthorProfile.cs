using AutoMapper;
using Core.ApiModels.InputModels.Author;
using Core.Authors.Queries.Common;
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

            CreateMap<Author, ViewModels.Author.ListAuthorModel>();

            CreateMap<Author, UpsertAuthorModel>()
                .ReverseMap();

            #region [Output Models]

            CreateMap<ListAuthorModel, ListAuthorModel>();

            #endregion
        }
    }
}
