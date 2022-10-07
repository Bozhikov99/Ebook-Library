﻿using System;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Queries.Book;
using Core.ViewModels.Book;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Handlers.BookHandlers
{
    public class GetAllBooksHandler : IRequestHandler<GetAllBooksQuery, IEnumerable<ListBookModel>>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GetAllBooksHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ListBookModel>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            string search = request.Search;
            string[] genres = request.Genres;

            IEnumerable<ListBookModel> books;

            if (search == null && genres.Length == 0)
            {
                books = await repository.All<Book>()
                    .ProjectTo<ListBookModel>(mapper.ConfigurationProvider)
                    .ToArrayAsync();
            }
            else if (search == null)
            {
                books = await repository.All(Search(genres))
                    .ProjectTo<ListBookModel>(mapper.ConfigurationProvider)
                    .ToArrayAsync();
            }
            else if (genres.Length == 0)
            {
                books = await repository.All(Search(search))
                    .ProjectTo<ListBookModel>(mapper.ConfigurationProvider)
                    .ToArrayAsync();
            }
            else
            {
                books = await repository.All(Search(search, genres))
                    .ProjectTo<ListBookModel>(mapper.ConfigurationProvider)
                    .ToArrayAsync();
            }

            return books;
        }

        private Expression<Func<Book, bool>> Search(string search)
        {
            Expression<Func<Book, bool>> searchExpression = b =>
                   b.Title.ToLower().Contains(search.ToLower()) ||
                   b.Genres.Any(g => g.Name.ToLower()
                                .Contains(search.ToLower())) ||
                   b.Author.FirstName.ToLower()
                                .Contains(search.ToLower()) ||
                   b.Author.LastName.ToLower()
                                .Contains(search.ToLower());

            return searchExpression;
        }

        private Expression<Func<Book, bool>> Search(string[] genres)
        {
            Expression<Func<Book, bool>> searchExpression = b =>
              b.Genres.Any(g => genres.Contains(g.Name));

            return searchExpression;
        }

        private Expression<Func<Book, bool>> Search(string search, string[] genres)
        {
            Expression<Func<Book, bool>> searchExpression = b =>
              b.Genres.Any(g => genres.Contains(g.Name)) &&
              (
                   b.Title.ToLower().Contains(search.ToLower()) ||
                   b.Genres.Any(g => g.Name.ToLower()
                                .Contains(search.ToLower())) ||
                   b.Author.FirstName.ToLower()
                                .Contains(search.ToLower()) ||
                   b.Author.LastName.ToLower()
                                .Contains(search.ToLower()));

            return searchExpression;
        }
    }
}
