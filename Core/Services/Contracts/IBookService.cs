using Core.ViewModels.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Contracts
{
    public interface IBookService
    {
        Task<IEnumerable<ListBookModel>> GetAll();

        Task Create(CreateBookModel model);

        Task<EditBookModel> GetEditModel(string id);

        Task Edit(EditBookModel model);

        Task<BookDetailsModel> Details(string id);

        Task<byte[]> GetContent(string id);

        Task Delete(string id);

    }
}
