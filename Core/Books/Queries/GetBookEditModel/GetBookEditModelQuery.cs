using Core.ViewModels.Book;
using Infrastructure.Persistance;

namespace Core.Books.Queries.GetBookEditModel
{
    public class GetBookEditModelQuery : IRequest<EditBookModel>
    {
        public string Id { get; set; } = null!;
    }
    public class GetEditModelHandler : IRequestHandler<GetBookEditModelQuery, EditBookModel>
    {
        private readonly EbookDbContext context;

        public GetEditModelHandler(EbookDbContext context)
        {
            this.context = context;
        }

        public async Task<EditBookModel> Handle(GetBookEditModelQuery request, CancellationToken cancellationToken)
        {
            string id = request.Id;

            EditBookModel? model = await context.Books
                .Select(b => new EditBookModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    AuthorId = b.AuthorId,
                    Description = b.Description,
                    GenreIds = b.Genres
                        .Select(g => g.Id),
                    Cover = b.Cover,
                    Content = b.Content,
                    ReleaseYear = b.ReleaseYear,
                    Pages = b.Pages
                })
                .FirstOrDefaultAsync(b => string.Equals(b.Id, id), cancellationToken);

            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return model;
        }
    }
}

