using AutoMapper;
using Core.ApiModels.Author;
using Core.Queries.Author;
using Core.ViewModels.Author;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Handlers.AuthorHandlers
{
    public class GetEditModelApiHandler : IRequestHandler<GetEditAuthorApiQuery, UpsertAuthorModel>
    {
        private readonly IMapper mapper;
        private readonly IRepository repository;

        public GetEditModelApiHandler(IMapper mapper, IRepository repository)
        {
            this.mapper = mapper;
            this.repository = repository;
        }
        public async Task<UpsertAuthorModel> Handle(GetEditAuthorApiQuery request, CancellationToken cancellationToken)
        {
            Author author = await repository.GetByIdAsync<Author>(request.Id);
            UpsertAuthorModel model = mapper.Map<UpsertAuthorModel>(author);

            ArgumentNullException.ThrowIfNull(model);

            return model;
        }
    }
}
