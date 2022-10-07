using System;
using Common.MessageConstants;
using Core.Queries.Book;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;

namespace Core.Handlers.BookHandlers
{
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

