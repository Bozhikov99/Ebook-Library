using AutoMapper;
using Core.Genres.Queries.Common;
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

            #region [Output Models]

            CreateMap<ListGenreModel, GenreModel>();

            #endregion
        }
    }
}
