using AutoMapper;
using Core.ViewModels.Book;
using Domain.Entities;
using Infrastructure.Common;

namespace Core.Books.Commands.Edit
{
    public class EditBookCommand : IRequest<bool>
    {
        public EditBookCommand(EditBookModel model)
        {
            Model = model;
        }

        public EditBookModel Model { get; set; }
    }

    public class EditBookHandler : IRequestHandler<EditBookCommand, bool>
    {
        private readonly IRepository repository;

        public EditBookHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(EditBookCommand request, CancellationToken cancellationToken)
        {
            bool isEdited = false;
            EditBookModel model = request.Model;

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

            isEdited = true;

            return isEdited;
        }
    }
}

