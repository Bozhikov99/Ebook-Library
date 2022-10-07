using System;
using Core.Commands.BookCommands;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;

namespace Core.Handlers.BookHandlers
{
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

