using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Core.Commands.AuthorCommands;
using Core.Validators;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;

namespace Core.Handlers.AuthorHandlers
{
    public class CreateAuthorHandler: IRequestHandler<CreateAuthorCommand, bool>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly AuthorValidator validator;

        public CreateAuthorHandler(IRepository repository, IMapper mapper, AuthorValidator validator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task<bool> Handle(
            CreateAuthorCommand request,
            CancellationToken cancellationToken)
        {
            bool isCreated = false;
            Author author = mapper.Map<Author>(request.Model);
            await validator.ValidateAuthorName(author.FirstName, author.LastName);

            await repository.AddAsync(author);
            await repository.SaveChangesAsync();

            isCreated = true;

            return isCreated;
        }
    }
}

