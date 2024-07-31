using Common.MessageConstants;
using Domain.Entities;
using Infrastructure.Persistance;

namespace Core.Authors.Commands.Create
{
    public class CreateAuthorCommand : IRequest<string>
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;
    }

    public class CreateAuthorHandler : IRequestHandler<CreateAuthorCommand, string>
    {
        private readonly EbookDbContext context;

        public CreateAuthorHandler(EbookDbContext context)
        {
            this.context = context;
        }

        public async Task<string> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            string firstName = request.FirstName;
            string lastName = request.LastName;

            bool isExisting = await context.Authors
                .AnyAsync(a => a.FirstName == firstName && a.LastName == lastName);

            if (isExisting)
            {
                throw new ArgumentException(ErrorMessageConstants.AUTHOR_EXISTS);
            }

            Author author = new Author
            {
                FirstName = firstName,
                LastName = lastName
            };

            await context.Authors
                .AddAsync(author, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return author.Id;
        }
    }
}
