using Domain.Entities;
using Infrastructure.Persistance;

namespace Core.Books.Commands.Delete
{
    public class DeleteBookCommand : IRequest
    {
        public string Id { get; set; } = null!;
    }

    public class DeleteBookHandler : IRequestHandler<DeleteBookCommand>
    {
        private readonly EbookDbContext context;

        public DeleteBookHandler(EbookDbContext context)
        {
            this.context = context;
        }

        //TODO: Implement handling of a single query book deletion
        public async Task<Unit> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            string id = request.Id;

            bool isExisting = await context.Books
                .AnyAsync(b => b.Id == id);

            if (!isExisting)
            {
                throw new ArgumentNullException(nameof(Book), id);
            }


            context.Books
                .Remove(new Book { Id = id });

            await context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}

