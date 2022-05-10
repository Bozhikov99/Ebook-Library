using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Core.ViewModels.Book;
using Infrastructure.Common;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Contracts
{
    public class BookService : IBookService
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        public BookService(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ListBookModel>> GetAll()
        {
            IEnumerable<ListBookModel> books = await repository.All<Book>()
                .ProjectTo<ListBookModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return books;
        }

        public async Task Create(CreateBookModel model)
        {
            await ValidateTitle(model.Title);

            Book book = mapper.Map<Book>(model);
            List<Genre> genres = new List<Genre>();

            foreach (var g in model.GenreIds)
            {
                Genre currentGenre = await repository.GetByIdAsync<Genre>(g);
                genres.Add(currentGenre);
            }

            book.Genres = genres;

            await repository.AddAsync(book);
            await repository.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            await repository.DeleteAsync<Book>(id);
            await repository.SaveChangesAsync();
        }

        public async Task<EditBookModel> GetEditModel(string id)
        {
            Book book = await repository.GetByIdAsync<Book>(id);
            EditBookModel model = mapper.Map<EditBookModel>(book);

            return model;
        }

        public async Task Edit(EditBookModel model)
        {
            Book book = await repository.All<Book>(b => b.Id == model.Id)
                .Include(b => b.Genres)
                .FirstAsync();

            book.Genres.Clear();

            List<Genre> genres = new List<Genre>();

            foreach (string id in model.GenreIds)
            {
                Genre currentGenre = await repository.GetByIdAsync<Genre>(id);
                genres.Add(currentGenre);
            }

            book.Genres = genres;

            await repository.SaveChangesAsync();
        }

        private async Task<bool> ValidateTitle(string title)
        {
            bool isExisting = await repository.All<Book>()
               .AnyAsync(b => b.Title == title);

            if (isExisting)
            {
                throw new ArgumentException(ErrorMessageConstants.BOOK_EXISTS);
            }

            return isExisting;
        }

        public async Task<BookDetailsModel> Details(string id)
        {
            Book book = repository.All<Book>(b=>b.Id==id)
                .Include(b=>b.Genres)
                .Include(b=>b.Author)
                .First();

            BookDetailsModel model = mapper.Map<BookDetailsModel>(book);

            return model;
        }
    }
}
