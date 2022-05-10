using AutoMapper;
using Core.ViewModels.Book;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mapping
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<CreateBookModel, Book>();

            CreateMap<Book, ListBookModel>();
        }
    }
}
