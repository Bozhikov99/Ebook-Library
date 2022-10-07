using System;
using AutoMapper;
using Core.Helpers;
using Core.Queries.User;
using Core.ViewModels.User;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;

namespace Core.Handlers.UserHandlers
{
    public class GetUserProfileHandler : IRequestHandler<GetUserProfileQuery, UserProfileModel>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly UserIdHelper helper;

        public GetUserProfileHandler(IRepository repository, IMapper mapper, UserIdHelper helper)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.helper = helper;
        }

        public async Task<UserProfileModel> Handle(
            GetUserProfileQuery request,
            CancellationToken cancellationToken)
        {
            User user = await repository.GetByIdAsync<User>(helper.GetUserId());
            UserProfileModel profile = mapper.Map<UserProfileModel>(user);

            return profile;
        }
    }
}

