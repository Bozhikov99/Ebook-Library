using Core.Genres.Queries.Common;
using Infrastructure.Persistance;

namespace Core.Genres.Queries.GetGenres
{
    public class GetGenresQuery : IRequest<IEnumerable<GenreModel>>
    {
    }

    public class GetAllGenresHandler : IRequestHandler<GetGenresQuery, IEnumerable<GenreModel>>
    {
        private readonly EbookDbContext context;

        public GetAllGenresHandler(EbookDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<GenreModel>> Handle(
            GetGenresQuery request,
            CancellationToken cancellationToken)
        {
            IEnumerable<GenreModel> genres = await context.Genres
                .Select(g => new GenreModel
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .ToArrayAsync();

            return genres;
        }
    }

}

