using Common.MessageConstants;
using Core.Genres.Queries.Common;
using Infrastructure.Persistance;

namespace Core.Genres.Queries.GetEditModelQuery
{
    public class GetEditModelQuery : IRequest<GenreModel>
    {
        public string Id { get; set; } = null!;
    }
    public class GetEditModelQueryHandler : IRequestHandler<GetEditModelQuery, GenreModel>
    {
        private readonly EbookDbContext context;

        public GetEditModelQueryHandler(EbookDbContext context)
        {
            this.context = context;
        }

        public async Task<GenreModel> Handle(GetEditModelQuery request, CancellationToken cancellationToken)
        {
            GenreModel? genre = await context.Genres
                .Select(g => new GenreModel
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

            if (genre is null)
            {
                throw new ArgumentNullException(ErrorMessageConstants.INVALID_GENRE);
            }

            return genre;
        }
    }

}

