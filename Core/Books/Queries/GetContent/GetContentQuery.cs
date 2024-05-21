using Common.MessageConstants;
using Domain.Entities;
using Infrastructure.Common;

namespace Core.Books.Queries.GetContent
{
    public class GetContentQuery : IRequest<byte[]>
    {
        public string Id { get; set; } = null!;
    }

    public class GetContentHandler : IRequestHandler<GetContentQuery, byte[]>
    {
        private readonly IRepository repository;

        public GetContentHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<byte[]> Handle(GetContentQuery request, CancellationToken cancellationToken)
        {
            Book book = await repository.GetByIdAsync<Book>(request.Id);

            ArgumentNullException.ThrowIfNull(book, ErrorMessageConstants.BOOK_DOES_NOT_EXIST);

            return book.Content;
        }
    }
}

