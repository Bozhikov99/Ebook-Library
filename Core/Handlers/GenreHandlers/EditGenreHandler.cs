using System;
using AutoMapper;
using Core.Commands.GenreCommands;
using Core.Queries.Genre;
using Core.Validators;
using Core.ViewModels.Genre;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;

namespace Core.Handlers.GenreHandlers
{
    public class EditGenreHandler : IRequestHandler<EditGenreCommand, bool>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly GenreValidator validator;

        public EditGenreHandler(IRepository repository, IMapper mapper, GenreValidator validator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task<bool> Handle(EditGenreCommand request, CancellationToken cancellationToken)
        {
            bool isEdited = false;

            Genre genre = mapper.Map<Genre>(request.Model);

            await validator.ValidateGenreName(genre.Name);
            repository.Update(genre);
            await repository.SaveChangesAsync();

            isEdited = true;

            return isEdited;
        }
    }
}

