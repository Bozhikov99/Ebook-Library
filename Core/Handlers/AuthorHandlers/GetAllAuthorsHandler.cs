using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Queries.Author;
using Core.ViewModels.Author;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Handlers.AuthorHandlers
{
    public class GetAllAuthorsHandler: IRequestHandler<GetAllAuthorsQuery, IEnumerable<ListAuthorModel>>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GetAllAuthorsHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ListAuthorModel>> Handle(
            GetAllAuthorsQuery request,
            CancellationToken cancellationToken)
        {
            IEnumerable<ListAuthorModel> authors = await repository.All<Author>()
                .ProjectTo<ListAuthorModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return authors;
        }
    }
}

