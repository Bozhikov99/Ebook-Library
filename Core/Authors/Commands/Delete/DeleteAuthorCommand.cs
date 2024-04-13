using Domain.Entities;
using Infrastructure;

namespace Core.Authors.Commands.Delete
{
    public class DeleteAuthorCommand : IRequest
    {
        public string Id { get; set; } = null!;
    }

    public class DeleteAuthorHandler : IRequestHandler<DeleteAuthorCommand>
    {
        private readonly EbookDbContext context;

        public DeleteAuthorHandler(EbookDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            Author author = new Author { Id = request.Id };

            context.Authors
                .Remove(author);

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}