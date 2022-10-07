using System;
using AutoMapper;
using Core.Commands.GenreCommands;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;

namespace Core.Handlers.GenreHandlers
{
    public class DeleteGenreHandler : IRequestHandler<DeleteGenreCommand, bool>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public DeleteGenreHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<bool> Handle(
            DeleteGenreCommand request,
            CancellationToken cancellationToken)
        {
            bool isDeleted = false;

            await repository.DeleteAsync<Genre>(request.Id);
            await repository.SaveChangesAsync();

            isDeleted = true;

            return isDeleted;
        }
    }
}

