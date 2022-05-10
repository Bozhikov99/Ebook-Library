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

            CreateMap<Book, ListBookModel>()
                .ForMember(d => d.Genres, s => s.MapFrom(b => b.Genres.Select(r => r.Name)))
                .ForMember(d => d.Rating,
                    s => s.MapFrom(b => b.Reviews.Count == 0 ? 0 : b.Reviews
                         .Select(r => r.Value)
                         .Sum() / b.Reviews.Count));

            CreateMap<Book, EditBookModel>()
                .ReverseMap();
        }
    }
}
