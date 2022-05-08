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
        //Create
        Task Create(CreateBookModel model);
        //Edit

        //Delete

        //List
    }
}
