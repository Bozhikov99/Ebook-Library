using System;
using Common.MessageConstants;
using Domain.Entities;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Core.Validators
{
    public class AuthorValidator
    {
        private readonly IRepository repository;

        public AuthorValidator(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task ValidateAuthorName(string firstName, string lastName)
        {
            bool isExisting = await repository.All<Author>()
               .AnyAsync(t => t.FirstName == firstName && t.LastName == lastName);

            if (isExisting)
            {
                throw new ArgumentException(ErrorMessageConstants.AUTHOR_EXISTS);
            }
        }
    }
}

