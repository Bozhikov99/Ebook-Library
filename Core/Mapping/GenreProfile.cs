﻿using AutoMapper;
using Core.ApiModels.InputModels.Genre;
using Core.ApiModels.OutputModels.Genre;
using Core.ViewModels.Genre;
using Domain.Entities;

namespace Core.Mapping
{
    public class GenreProfile : Profile
    {
        public GenreProfile()
        {
            CreateMap<CreateGenreModel, Genre>();

            CreateMap<EditGenreModel, Genre>()
                .ReverseMap();

            CreateMap<Genre, ListGenreModel>();

            CreateMap<Genre, UpsertGenreModel>()
                .ReverseMap();

            #region [Output Models]

            CreateMap<ListGenreModel, ListGenreOutputModel>();

            #endregion
        }
    }
}
