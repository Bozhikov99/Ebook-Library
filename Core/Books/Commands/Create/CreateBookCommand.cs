using Domain.Entities;
using Infrastructure.Persistance;

namespace Core.Books.Commands.Create
{
    public class CreateBookCommand : IRequest
    {
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int ReleaseYear { get; set; }

        public int Pages { get; set; }

        public string AuthorId { get; set; } = null!;

        //To be validated in a validator
        public byte[] Cover { get; set; } = null!;

        //To be validated in a validator
        public byte[] Content { get; set; } = null!;

        public IEnumerable<string> GenreIds { get; set; } = new List<string>();
    }

    public class CreateBookHandler : IRequestHandler<CreateBookCommand>
    {
        private readonly EbookDbContext context;

        public CreateBookHandler(EbookDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            bool isExistingTitle = await context.Books
                .AnyAsync(x => x.Title == request.Title, cancellationToken);

            if (isExistingTitle)
            {
                throw new ArgumentException();
            }

            bool isExistingAuthor = await context.Authors
                .AnyAsync(x => x.Id == request.AuthorId, cancellationToken);

            if (!isExistingAuthor)
            {
                throw new ArgumentException();
            }

            IEnumerable<string> genreIds = request.GenreIds;

            ICollection<Genre> genres = await context.Genres
                .Select(g => new Genre { Id = g.Id })
                .Where(g => genreIds.Contains(g.Id))
                .ToListAsync(cancellationToken);
            //TODO: Add validation
            Book book = new()
            {
                Title = request.Title,
                Description = request.Description,
                ReleaseYear = request.ReleaseYear,
                Pages = request.Pages,
                AuthorId = request.AuthorId,
                Cover = request.Cover,
                Content = request.Content
            };

            ICollection<BookGenre> bookGenres = genres
                .Select(g => new BookGenre
                {
                    GenreId = g.Id,
                    Book = book
                })
                .ToList();

            book.BookGenres = bookGenres;

            await context.AddAsync(book);
            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}

