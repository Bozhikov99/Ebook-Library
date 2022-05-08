using AutoMapper;
using AutoMapper.QueryableExtensions;
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
            await repository.DeleteAsync<Author>(id);
            await repository.SaveChangesAsync();
        }

        public async Task<EditAuthorModel> GetEditModel(string id)
        {
            Author author = await repository.GetByIdAsync<Author>(id);
            EditAuthorModel model = mapper.Map<EditAuthorModel>(author);

            return model;
        }

        public async Task EditAuthor(EditAuthorModel model)
        {
            Author author = mapper.Map<Author>(model);

            await ValidateAuthorName(author.FirstName, author.LastName);

            repository.Update(author);
            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ListAuthorModel>> GetAllAuthors()
        {
            IEnumerable<ListAuthorModel> authors = await repository.All<Author>()
                .ProjectTo<ListAuthorModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return authors;
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
