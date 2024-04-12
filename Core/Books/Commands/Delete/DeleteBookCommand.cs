using Domain.Entities;
using Infrastructure.Common;

namespace Core.Books.Commands.Delete
{
    public class DeleteBookCommand : IRequest<bool>
    {
        public DeleteBookCommand(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }

    public class DeleteBookHandler : IRequestHandler<DeleteBookCommand, bool>
    {
        private readonly IRepository repository;

        public DeleteBookHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            bool isDelete = false;

            await repository.DeleteAsync<Book>(request.Id);
            await repository.SaveChangesAsync();

            isDelete = true;

            return isDelete;
        }
    }
}

