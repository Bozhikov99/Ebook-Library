using Core.ViewModels.Book;

namespace Core.Services.Contracts
{
    public interface IBookService
    {
        Task<IEnumerable<ListBookModel>> GetAll(int p);

        Task<IEnumerable<ListBookModel>> GetAll(int p, string search);

        Task<IEnumerable<ListBookModel>> GetAll(int p, string[] genres);

        Task<IEnumerable<ListBookModel>> GetAll(int p, string search, string[] genres);

        Task Create(CreateBookModel model);

        Task<EditBookModel> GetEditModel(string id);

        Task Edit(EditBookModel model);

        Task<BookDetailsModel> Details(string id);

        Task<byte[]> GetContent(string id);

        Task Delete(string id);

    }
}
