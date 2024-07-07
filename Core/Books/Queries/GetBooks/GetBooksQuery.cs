using Domain.Entities;
using Infrastructure.Persistance;

namespace Core.Books.Queries.GetBooks
{
    public class GetBooksQuery : IRequest<IEnumerable<BookModel>>
    {
        public string? Search { get; private set; }

        public HashSet<string> GenreIds { get; set; } = new HashSet<string>();

        public int PageNumber { get; set; }

        public int PageSize { get; set; } = 4;
    }

    public class GetAllBooksApiHandler : IRequestHandler<GetBooksQuery, IEnumerable<BookModel>>
    {
        private readonly EbookDbContext context;

        public GetAllBooksApiHandler(EbookDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<BookModel>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            string? search = request.Search?
                .ToLower();

            HashSet<string> genreIds = request.GenreIds;

            IQueryable<Book> books = context.Books;

            if (!string.IsNullOrWhiteSpace(search))
            {
                books = books.Where(b => b.Title.ToLower().Contains(search) || b.Description.ToLower().Contains(search));
            }

            if (genreIds.Count > 0)
            {
                books = books.Where(b => b.BookGenres.Any(bg => genreIds.Contains(bg.GenreId)));
            }

            IEnumerable<BookModel> dtos = await books.Select(b => new BookModel
            {
                Id = b.Id,
                Title = b.Title,
                Cover = b.Cover,
                ReleaseYear = b.ReleaseYear,
                Rating = b.Reviews.Count == 0 ? 0 :
                         b.Reviews.Select(r => r.Value)
                         .Sum() / b.Reviews.Count,
                Genres = b.BookGenres.Select(bg => bg.Genre.Name),
                Author = $"{b.Author.FirstName} {b.Author.LastName}"
            })
                .Skip(request.PageNumber * (request.PageSize - 1))
                .Take(request.PageSize)
                .ToArrayAsync(cancellationToken);

            return dtos;
        }
    }
}
