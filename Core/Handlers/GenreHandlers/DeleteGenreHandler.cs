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
            await repository.DeleteAsync<Genre>(request.Id);
            int commits = await repository.SaveChangesAsync();

            bool isDeleted = commits > 0;

            return isDeleted;
        }
    }
}

