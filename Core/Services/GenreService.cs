using Core.Services.Contracts;
using Core.ViewModels.Genre;
using Infrastructure.Common;
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

        public GenreService(IRepository repository)
        {
            this.repository = repository;
        }

        public Task CreateGenre(CreateGenreModel model)
        {
            throw new NotImplementedException();
        }

        public Task DeleteGenre(string id)
        {
            throw new NotImplementedException();
        }

        public Task EditGenre(EditGenreModel model)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ListGenreModel>> GetAllGenres()
        {
            throw new NotImplementedException();
        }
    }
}
