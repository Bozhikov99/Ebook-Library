using System;
using AutoMapper;
using Core.Commands.AuthorCommands;
using Core.Validators;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;

namespace Core.Handlers.AuthorHandlers
{
    public class EditAuthorHandler: IRequestHandler<EditAuthorCommand, bool>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly AuthorValidator validator;

        public EditAuthorHandler(IRepository repository, IMapper mapper, AuthorValidator validator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task<bool> Handle(
            EditAuthorCommand request,
            CancellationToken cancellationToken)
        {
            bool isEdited = false;

            Author author = mapper.Map<Author>(request.Model);
            await validator.ValidateAuthorName(author.FirstName, author.LastName);

            repository.Update(author);
            await repository.SaveChangesAsync();

            isEdited = true;

            return isEdited;

        }
    }
}

