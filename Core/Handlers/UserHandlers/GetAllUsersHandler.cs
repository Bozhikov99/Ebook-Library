using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Queries.User;
using Core.ViewModels.User;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Handlers.UserHandlers
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<ListUserModel>>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GetAllUsersHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ListUserModel>> Handle(
            GetAllUsersQuery request,
            CancellationToken cancellationToken)
        {
            IEnumerable<ListUserModel> users = await repository.All<User>()
                .ProjectTo<ListUserModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return users;
        }
    }
}

