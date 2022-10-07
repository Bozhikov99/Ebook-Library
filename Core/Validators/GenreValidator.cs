using System;
using Common.MessageConstants;
using Domain.Entities;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Core.Validators
{
    public class GenreValidator
    {
        private readonly IRepository repository;

        public GenreValidator(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task ValidateGenreName(string name)
        {
            bool isExisting = await repository.All<Genre>()
               .AnyAsync(t => t.Name == name);

            if (isExisting)
            {
                throw new ArgumentException(ErrorMessageConstants.GENRE_EXISTS);
            }
        }
    }
}

