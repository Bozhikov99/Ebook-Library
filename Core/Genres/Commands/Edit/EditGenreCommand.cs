using Common.MessageConstants;
using Domain.Entities;
using Infrastructure.Persistance;

namespace Core.Genres.Commands.Edit
{
    public class EditGenreCommand : IRequest
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;
    }

    public class EditGenreCommandHandler : IRequestHandler<EditGenreCommand>
    {
        private readonly EbookDbContext context;

        public EditGenreCommandHandler(EbookDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(EditGenreCommand request, CancellationToken cancellationToken)
        {
            bool isExistingGenre = await context.Genres
                .AnyAsync(g => string.Equals(g.Name, request.Name), cancellationToken);

            if (isExistingGenre)
            {
                throw new ArgumentException(ErrorMessageConstants.BOOK_EXISTS);
            }

            Genre? genre = await context.Genres
                .FirstOrDefaultAsync(g => string.Equals(g.Id, request.Id), cancellationToken);

            if (genre is null)
            {
                throw new ArgumentException(nameof(Genre), request.Id);
            }

            genre.Name = request.Name;

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

