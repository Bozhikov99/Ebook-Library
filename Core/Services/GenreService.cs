using AutoMapper;
using AutoMapper.QueryableExtensions;
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
            Genre genre = mapper.Map<Genre>(model);

            await repository.AddAsync(genre);
            await repository.SaveChangesAsync();
        }

        public Task DeleteGenre(string id)
        {
            throw new NotImplementedException();
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
