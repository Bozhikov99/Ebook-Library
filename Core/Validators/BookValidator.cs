using System;
using Common.MessageConstants;
using Domain.Entities;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Core.Validators
{
    public class BookValidator
    {
        private readonly IRepository repository;

        public BookValidator(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task ValidateTitle(string title)
        {
            bool isExisting = await repository.All<Book>()
               .AnyAsync(b => b.Title == title);

            if (isExisting)
            {
                throw new ArgumentException(ErrorMessageConstants.BOOK_EXISTS);
            }
        }
    }
}

