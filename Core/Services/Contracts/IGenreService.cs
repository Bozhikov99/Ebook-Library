using Core.ViewModels.Genre;

namespace Core.Services.Contracts
{
    public interface IGenreService
    {
        Task CreateGenre(CreateGenreModel model);

        Task<EditGenreModel> GetEditModel(string id);

        Task EditGenre(EditGenreModel model);

        Task DeleteGenre(string id);

        Task<IEnumerable<ListGenreModel>> GetAllGenres();
    }
}