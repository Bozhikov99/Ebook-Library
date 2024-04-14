using Core.Authors.Queries.Common;
using Infrastructure;

namespace Core.Authors.Queries.GetAuthors
{
    public class GetAuthorsQuery : IRequest<IEnumerable<AuthorModel>>
    {
    }

    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAuthorsQuery, IEnumerable<AuthorModel>>
    {
        private readonly EbookDbContext context;

        public GetAllAuthorsQueryHandler(EbookDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<AuthorModel>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<AuthorModel> authors = await context.Authors
                .Select(a => new AuthorModel
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName
                })
                .ToArrayAsync(cancellationToken);

            return authors;
        }
    }
}
