using Domain.Entities;
using Infrastructure.Common;

namespace Core.Books.Commands.Edit
{
    public class EditBookCommand : IRequest
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public byte[]? Cover { get; set; }

        public byte[]? Content { get; set; }

        public int ReleaseYear { get; set; }

        public int Pages { get; set; }

        public string AuthorId { get; set; } = null!;

        public IEnumerable<string> GenreIds { get; set; } = new List<string>();
    }

    public class EditBookHandler : IRequestHandler<EditBookCommand>
    {
        private readonly IRepository repository;

        public EditBookHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Unit> Handle(EditBookCommand request, CancellationToken cancellationToken)
        {
            string id = request.Id;

            //TODO: Select this
            Book book = await repository.All<Book>(b => string.Equals(b.Id, id))
                .Include(b => b.Genres)
                .FirstAsync();

            book.Genres.Clear();

            List<Genre> genres = new List<Genre>();

            IEnumerable<string> genreIds = request.GenreIds;

            foreach (string gId in genreIds)
            {
                //To be fixed
                Genre currentGenre = await repository.GetByIdAsync<Genre>(gId);
                genres.Add(currentGenre);
            }

            book.Genres = genres;
            book.Title = request.Title;
            book.Content = request.Content == null ? book.Content : request.Content;
            book.Cover = request.Cover == null ? book.Cover : request.Cover;
            book.AuthorId = request.AuthorId;
            book.ReleaseYear = request.ReleaseYear;
            book.Pages = request.Pages;

            await repository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}

