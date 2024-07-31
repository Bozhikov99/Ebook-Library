using Common.MessageConstants;
using Domain.Entities;
using Infrastructure.Persistance;

namespace Core.Genres.Commands.Create
{
    public class CreateGenreCommand : IRequest<string>
    {
        public string Name { get; set; } = null!;
    }

    public class CreateGenreHandler : IRequestHandler<CreateGenreCommand, string>
    {
        private readonly EbookDbContext context;

        public CreateGenreHandler(EbookDbContext context)
        {
            this.context = context;
        }

        public async Task<string> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
        {
            bool isExistingGenre = await context.Genres
                .AnyAsync(g => string.Equals(g.Name, request.Name), cancellationToken);

            if (isExistingGenre)
            {
                throw new ArgumentException(ErrorMessageConstants.BOOK_EXISTS);
            }

            Genre genre = new()
            {
                Name = request.Name
            };

            await context.Genres
                .AddAsync(genre, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return genre.Id;
        }
    }
}

