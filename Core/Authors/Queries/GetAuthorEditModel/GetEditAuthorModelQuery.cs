using Core.Authors.Queries.Common;
using Infrastructure;

namespace Core.Authors.Queries.GetEditModel
{
    public class GetEditAuthorModelQuery : IRequest<AuthorModel>
    {
        public string Id { get; set; } = null!;
    }

    public class GetEditAuthorModelQueryHandler : IRequestHandler<GetEditAuthorModelQuery, AuthorModel>
    {
        private readonly EbookDbContext context;

        public GetEditAuthorModelQueryHandler(EbookDbContext context)
        {
            this.context = context;
        }

        public async Task<AuthorModel> Handle(GetEditAuthorModelQuery request, CancellationToken cancellationToken)
        {
            AuthorModel? model = await context.Authors
                .Where(a => a.Id == request.Id)
                .Select(a => new AuthorModel
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            return model;
        }
    }
}
