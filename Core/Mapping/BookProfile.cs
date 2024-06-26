﻿using AutoMapper;
using Core.ApiModels.InputModels.Books;
using Core.ApiModels.OutputModels.Book;
using Core.Books.Queries.Details;
using Core.ViewModels.Book;
using Domain.Entities;

namespace Core.Mapping
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<CreateBookModel, Book>();

            CreateMap<BookInputModel, CreateBookModel>();

            CreateMap<BookInputModel, EditBookModel>();

            CreateMap<Book, BookInputModel>()
                .ForMember(d => d.Content, s => s.Ignore())
                .ForMember(d => d.Cover, s => s.Ignore());

            CreateMap<Book, ListBookModel>()
                .ForMember(d => d.Genres, s => s.MapFrom(b => b.Genres.Select(r => r.Name)))
                .ForMember(d => d.Author, s => s.MapFrom(b => $"{b.Author.FirstName} {b.Author.LastName}"))
                .ForMember(d => d.Rating,
                    s => s.MapFrom(b => b.Reviews.Count == 0 ? 0 : b.Reviews
                         .Select(r => r.Value)
                         .Sum() / b.Reviews.Count));

            CreateMap<Book, ListBookOutputModel>()
               .ForMember(d => d.Genres, s => s.MapFrom(b => b.Genres.Select(r => r.Name)))
               .ForMember(d => d.Author, s => s.MapFrom(b => $"{b.Author.FirstName} {b.Author.LastName}"))
               .ForMember(d => d.Rating,
                   s => s.MapFrom(b => b.Reviews.Count == 0 ? 0 : b.Reviews
                        .Select(r => r.Value)
                        .Sum() / b.Reviews.Count));

            CreateMap<Book, EditBookModel>()
                .ForMember(d => d.Content, s => s.Ignore())
                .ForMember(d => d.Cover, s => s.Ignore())
                .ReverseMap();

            CreateMap<Book, BookDetailsModel>()
                .ForMember(d => d.Genres, s => s.MapFrom(b => b.Genres.Select(r => r.Name)))
                .ForMember(d => d.Author, s => s.MapFrom(b => $"{b.Author.FirstName} {b.Author.LastName}"))
                .ForMember(d => d.Rating,
                    s => s.MapFrom(b => b.Reviews.Count == 0 ? 0 : b.Reviews
                         .Select(r => r.Value)
                         .Sum() / b.Reviews.Count));

            CreateMap<Book, BookDetailsApiModel>()
                .ForMember(d => d.Genres, s => s.MapFrom(b => b.Genres.Select(r => r.Name)))
                .ForMember(d => d.Author, s => s.MapFrom(b => $"{b.Author.FirstName} {b.Author.LastName}"))
                .ForMember(d => d.Reviews, s => s.MapFrom(b => b.Reviews))
                .ForMember(d => d.Rating,
                    s => s.MapFrom(b => b.Reviews.Count == 0 ? 0 : b.Reviews
                         .Select(r => r.Value)
                         .Sum() / b.Reviews.Count));

            CreateMap<Book, BookDetailsOutputModel>()
                .ForMember(d => d.Genres, s => s.MapFrom(b => b.Genres.Select(r => r.Name)))
                .ForMember(d => d.Author, s => s.MapFrom(b => $"{b.Author.FirstName} {b.Author.LastName}"))
                .ForMember(d => d.Reviews, s => s.MapFrom(b => b.Reviews))
                .ForMember(d => d.Rating,
                    s => s.MapFrom(b => b.Reviews.Count == 0 ? 0 : b.Reviews
                         .Select(r => r.Value)
                         .Sum() / b.Reviews.Count));
        }
    }
}
