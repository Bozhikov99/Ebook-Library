using System;
using AutoMapper;
using Core.Queries.Author;
using Core.ViewModels.Author;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;

namespace Core.Handlers.AuthorHandlers
{
    public class GetEditModelHandler: IRequestHandler<GetEditModelQuery, EditAuthorModel>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GetEditModelHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<EditAuthorModel> Handle(
            GetEditModelQuery request,
            CancellationToken cancellationToken)
        {
            Author author = await repository.GetByIdAsync<Author>(request.Id);
            EditAuthorModel model = mapper.Map<EditAuthorModel>(author);

            return model;
        }
    }
}

