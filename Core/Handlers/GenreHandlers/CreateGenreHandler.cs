using System;
using AutoMapper;
using Core.Commands.GenreCommands;
using Core.Validators;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;

namespace Core.Handlers.GenreHandlers
{
    public class CreateGenreHandler : IRequestHandler<CreateGenreCommand, string>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly GenreValidator validator;

        public CreateGenreHandler(IRepository repository, IMapper mapper, GenreValidator validator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task<string> Handle(
            CreateGenreCommand request,
            CancellationToken cancellationToken)
        {

            Genre genre = mapper.Map<Genre>(request.Model);

            await validator.ValidateGenreName(genre.Name);
            await repository.AddAsync(genre);
            await repository.SaveChangesAsync();

            string id = repository
                .AllReadonly<Genre>(g => g.Name == genre.Name)
                .First(g=>g.Name==genre.Name)
                .Id;

            return id;
        }
    }
}

