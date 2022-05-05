using Core.ViewModels.Genre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Contracts
{
    public interface IGenreService
    {
        Task CreateGenre(CreateGenreModel model);

        Task EditGenre(EditGenreModel model);

        Task DeleteGenre(string id);

        Task<IEnumerable<ListGenreModel>> GetAllGenres();
    }
}