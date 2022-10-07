using System;
using AutoMapper;
using Core.Commands.AuthorCommands;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;

namespace Core.Handlers.AuthorHandlers
{
    public class DeleteAuthorHandler : IRequestHandler<DeleteAuthorCommand, bool>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public DeleteAuthorHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<bool> Handle(
            DeleteAuthorCommand request,
            CancellationToken cancellationToken)
        {
            bool isDeleted = false;

            await repository.DeleteAsync<Author>(request.Id);
            await repository.SaveChangesAsync();

            isDeleted = true;

            return isDeleted;
        }
    }
}

