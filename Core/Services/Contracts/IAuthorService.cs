using Core.ViewModels.Author;

namespace Core.Services.Contracts
{
    public interface IAuthorService
    {
        Task CreateAuthor(CreateAuthorModel model);

        Task EditAuthor(EditAuthorModel model);

        Task Delete(string id);

        Task<IEnumerable<ListAuthorModel>> GetAllAuthors();
    }
}
