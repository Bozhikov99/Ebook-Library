using Domain.Entities;
using Infrastructure.Persistance;

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
        private readonly EbookDbContext context;

        public EditBookHandler(EbookDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(EditBookCommand request, CancellationToken cancellationToken)
        {
            string id = request.Id;

            //TODO: Select this
            Book? book = await context.Books
                .Select(b => new Book
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    Cover = b.Cover,
                    ReleaseYear = b.ReleaseYear,
                    Pages = b.Pages,
                    AuthorId = b.AuthorId,
                    BookGenres = b.BookGenres,
                    Author = b.Author
                })
                .FirstOrDefaultAsync(b => string.Equals(b.Id, id), cancellationToken);

            if (book is null)
            {
                throw new ArgumentException();
            }

            byte[] cover = request.Cover ?? book.Cover;
            byte[] content = request.Content ?? book.Content;

            IEnumerable<string> genreIds = request.GenreIds;

            ICollection<Genre> genres = await context.Genres
                .Select(g => new Genre { Id = g.Id })
                .Where(g => genreIds.Contains(g.Id))
                .ToListAsync(cancellationToken);

            context.BookGenres
                .RemoveRange(book.BookGenres);

            List<BookGenre> newGenres = genres
                .Select(g => new BookGenre
                {
                    GenreId = g.Id,
                    BookId = book.Id
                })
                .ToList();

            await context.BookGenres
                .AddRangeAsync(newGenres, cancellationToken);

            book.Title = request.Title;
            book.Content = content;
            book.Cover = cover;
            book.AuthorId = request.AuthorId;
            book.ReleaseYear = request.ReleaseYear;
            book.Pages = request.Pages;

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

