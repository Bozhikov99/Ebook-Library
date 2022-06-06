using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.MessageConstants;
using Core.Services.Contracts;
using Core.ViewModels.Genre;
using Infrastructure.Common;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

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
            await ValidateGenreName(model.Name);

            Genre genre = mapper.Map<Genre>(model);

            await repository.AddAsync(genre);
            await repository.SaveChangesAsync();
        }

        public async Task DeleteGenre(string id)
        {
            await repository.DeleteAsync<Genre>(id);
            await repository.SaveChangesAsync();
        }

        public async Task EditGenre(EditGenreModel model)
        {
            await ValidateGenreName(model.Name);

            Genre genre = mapper.Map<Genre>(model);

            repository.Update(genre);
            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ListGenreModel>> GetAllGenres()
        {
            IEnumerable<ListGenreModel> genres = await repository.All<Genre>()
                .ProjectTo<ListGenreModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return genres;
        }

        public async Task<EditGenreModel> GetEditModel(string id)
        {
            Genre genre = await repository.GetByIdAsync<Genre>(id);

            if (genre == null)
            {
                throw new ArgumentNullException(ErrorMessageConstants.INVALID_GENRE);
            }

            EditGenreModel model = mapper.Map<EditGenreModel>(genre);

            return model;
        }

        private async Task ValidateGenreName(string name)
        {
            bool isExisting = await repository.All<Genre>()
               .AnyAsync(t => t.Name == name);

            if (isExisting)
            {
                throw new ArgumentException(ErrorMessageConstants.GENRE_EXISTS);
            }
        }
    }
}
