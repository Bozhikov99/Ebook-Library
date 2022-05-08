using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Book
{
    public class FileWrapper
    {
        public IFormFile File { get; set; }
    }
}
