using AutoMapper;
using Common.MessageConstants;
using Core.ApiModels.OutputModels.User;
using Core.Queries.User;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Handlers.UserHandlers
{
    public class GetProfileHandler : IRequestHandler<GetProfileQuery, UserProfileOutputModel>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GetProfileHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<UserProfileOutputModel> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            string id = request.Id;
            User user = await repository.GetByIdAsync<User>(id);

            UserProfileOutputModel outputModel = mapper.Map<UserProfileOutputModel>(user);

            return outputModel;
        }
    }
}
