using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Core.Services.Contracts;
using Core.ViewModels.Genre;
using Infrastructure.Common;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class GenreService : IGenreService
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GenreService(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task CreateGenre(CreateGenreModel model)
        {
            bool isExisting = await repository.All<Genre>()
                .AnyAsync(t => t.Name == model.Name);

            if (isExisting)
            {
                throw new ArgumentException(ErrorMessageConstants.GENRE_EXISTS);
            }

            Genre genre = mapper.Map<Genre>(model);

            await repository.AddAsync(genre);
            await repository.SaveChangesAsync();
        }

        public async Task DeleteGenre(string id)
        {
            await repository.DeleteAsync<Genre>(id);
            await repository.SaveChangesAsync();
        }

        public Task EditGenre(EditGenreModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ListGenreModel>> GetAllGenres()
        {
            IEnumerable<ListGenreModel> genres = await repository.All<Genre>()
                .ProjectTo<ListGenreModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return genres;
        }
    }
}
