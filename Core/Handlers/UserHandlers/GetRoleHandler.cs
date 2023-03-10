using AutoMapper;
using Common.MessageConstants;
using Core.ApiModels.User;
using Core.Queries.User;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Handlers.UserHandlers
{
    public class GetRoleHandler : IRequestHandler<GetRoleQuery, RoleInfoModel>
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GetRoleHandler(IRepository repository, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            this.roleManager = roleManager;
            this.mapper = mapper;
            this.repository = repository;
        }

        public async Task<RoleInfoModel> Handle(GetRoleQuery request, CancellationToken cancellationToken)
        {
            string id = request.Id;

            IdentityRole role = await roleManager.FindByIdAsync(id);

            if (role is null)
            {
                throw new ArgumentNullException(ErrorMessageConstants.ROLE_NOT_FOUND);
            }

            RoleInfoModel model = mapper.Map<RoleInfoModel>(role);

            return model;
        }
    }
}
