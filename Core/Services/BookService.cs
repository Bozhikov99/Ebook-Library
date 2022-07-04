using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.MessageConstants;
using Core.Services.Contracts;
using Core.ViewModels.Book;
using Infrastructure.Common;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.Services
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

        public async Task<IEnumerable<ListBookModel>> GetAll(int p)
        {
            IEnumerable<ListBookModel> books = await repository.All<Book>()
                .ProjectTo<ListBookModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return books;
        }

        public async Task<IEnumerable<ListBookModel>> GetAll(int p, string[] genres)
        {
            IEnumerable<ListBookModel> books = await repository.All(Search(genres))
                .ProjectTo<ListBookModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return books;
        }

        public async Task<IEnumerable<ListBookModel>> GetAll(int p, string search)
        {
            IEnumerable<ListBookModel> books = await repository.All(Search(search))
                .ProjectTo<ListBookModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return books;
        }

        public async Task<IEnumerable<ListBookModel>> GetAll(int p, string search, string[] genres)
        {
            IEnumerable<ListBookModel> books = await repository.All(Search(search, genres))
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
            book.Title = model.Title;
            book.Content = model.Content == null ? book.Content : model.Content;
            book.Cover = model.Cover == null ? book.Cover : model.Cover;
            book.AuthorId = model.AuthorId;
            book.ReleaseYear = model.ReleaseYear;
            book.Pages = model.Pages;

            await repository.SaveChangesAsync();
        }

        public async Task<byte[]> GetContent(string id)
        {
            Book book = await repository.GetByIdAsync<Book>(id);

            ArgumentNullException.ThrowIfNull(book, ErrorMessageConstants.BOOK_DOES_NOT_EXIST);

            return book.Content;
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
            Book book = repository.All<Book>(b => b.Id == id)
                .Include(b => b.Genres)
                .Include(b => b.Author)
                .Include(b=> b.Reviews)
                .First();

            BookDetailsModel model = mapper.Map<BookDetailsModel>(book);

            return model;
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
