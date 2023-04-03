using Core.ViewModels.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ApiModels.InputModels.Books
{
    public class EditBookResponseModel
    {
        public string Id { get; set; }

        public BookInputModel Model { get; set; }

        public BookInputDataModel BookData { get; set; }
    }
}
