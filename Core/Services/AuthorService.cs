using AutoMapper;
using Common;
using Core.Services.Contracts;
using Core.ViewModels.Author;
using Infrastructure.Common;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public AuthorService(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task CreateAuthor(CreateAuthorModel model)
        {
            await ValidateAuthorName(model.FirstName, model.LastName);

            Author author = mapper.Map<Author>(model);

            await repository.AddAsync(author);
            await repository.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task EditAuthor(EditAuthorModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ListAuthorModel>> GetAllAuthors()
        {
            throw new NotImplementedException();
        }

        private async Task ValidateAuthorName(string firstName, string lastName)
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
