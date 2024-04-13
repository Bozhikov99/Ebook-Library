using Domain.Entities;
using Infrastructure;

namespace Core.Authors.Commands.Edit
{
    public class EditAuthorCommand : IRequest
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Id { get; set; } = null!;
    }

    public class EditAuthorApiHandler : IRequestHandler<EditAuthorCommand>
    {
        private readonly EbookDbContext context;

        public EditAuthorApiHandler(EbookDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(EditAuthorCommand request, CancellationToken cancellationToken)
        {
            string id = request.Id;

            Author? author = await context.Authors
                .FirstOrDefaultAsync(a => string.Equals(a.Id, id));

            if (author is null)
            {
                throw new ArgumentNullException();
            }

            author.FirstName = request.FirstName;
            author.LastName = request.LastName;

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
