using Domain.Entities;
using Infrastructure.Persistance;

namespace Core.Genres.Commands.Delete
{
    public class DeleteGenreCommand : IRequest
    {
        public string Id { get; set; } = null!;
    }

    public class DeleteGenreHandler : IRequestHandler<DeleteGenreCommand>
    {
        private readonly EbookDbContext context;

        public DeleteGenreHandler(EbookDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
        {
            Genre? genre = await context.Genres
                .FirstOrDefaultAsync(g => string.Equals(g.Id, request.Id), cancellationToken);

            if (genre is null)
            {
                throw new ArgumentException();
            }

            context.Genres.Remove(genre);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

