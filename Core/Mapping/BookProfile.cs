using AutoMapper;
using Core.ViewModels.Book;
using Domain.Entities;

namespace Core.Mapping
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<CreateBookModel, Book>();

            CreateMap<Book, ListBookModel>()
                .ForMember(d => d.Genres, s => s.MapFrom(b => b.Genres.Select(r => r.Name)))
                .ForMember(d => d.Author, s => s.MapFrom(b => $"{b.Author.FirstName} {b.Author.LastName}"))
                .ForMember(d => d.Rating,
                    s => s.MapFrom(b => b.Reviews.Count == 0 ? 0 : b.Reviews
                         .Select(r => r.Value)
                         .Sum() / b.Reviews.Count));

            CreateMap<Book, EditBookModel>()
                .ReverseMap();

            CreateMap<Book, BookDetailsModel>()
                .ForMember(d => d.Genres, s => s.MapFrom(b => b.Genres.Select(r => r.Name)))
                .ForMember(d => d.Author, s => s.MapFrom(b => $"{b.Author.FirstName} {b.Author.LastName}"))
                .ForMember(d => d.Rating,
                    s => s.MapFrom(b => b.Reviews.Count == 0 ? 0 : b.Reviews
                         .Select(r => r.Value)
                         .Sum() / b.Reviews.Count));
        }
    }
}
