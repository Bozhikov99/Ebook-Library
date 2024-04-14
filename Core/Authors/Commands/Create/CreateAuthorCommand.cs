using Common.MessageConstants;
using Domain.Entities;
using Infrastructure.Common;

namespace Core.Authors.Commands.Create
{
    public class CreateAuthorCommand : IRequest<string>
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;
    }

    public class CreateAuthorHandler : IRequestHandler<CreateAuthorCommand, string>
    {
        private readonly IRepository repository;

        public CreateAuthorHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<string> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            string firstName = request.FirstName;
            string lastName = request.LastName;

            bool isExisting = await repository.All<Author>()
                .AnyAsync(t => t.FirstName == firstName && t.LastName == lastName);

            if (isExisting)
            {
                throw new ArgumentException(ErrorMessageConstants.AUTHOR_EXISTS);
            }

            Author author = new Author
            {
                FirstName = firstName,
                LastName = lastName
            };

            await repository.AddAsync(author);
            await repository.SaveChangesAsync();

            return author.Id;
        }
    }
}
