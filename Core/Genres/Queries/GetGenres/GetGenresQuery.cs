using Core.ViewModels.Genre;
using Infrastructure.Persistance;

namespace Core.Genres.Queries.GetGenres
{
    public class GetGenresQuery : IRequest<IEnumerable<ListGenreModel>>
    {
    }
    public class GetAllGenresHandler : IRequestHandler<GetGenresQuery, IEnumerable<ListGenreModel>>
    {
        private readonly EbookDbContext context;

        public GetAllGenresHandler(EbookDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<ListGenreModel>> Handle(
            GetGenresQuery request,
            CancellationToken cancellationToken)
        {
            IEnumerable<ListGenreModel> genres = await context.Genres
                .Select(g => new ListGenreModel
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .ToArrayAsync();

            return genres;
        }
    }

}

