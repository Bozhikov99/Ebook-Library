using AutoMapper;
using Common.MessageConstants;
using Core.Commands.UserCommands;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Core.Handlers.UserHandlers
{
    public class EditRolesHandler: IRequestHandler<EditRolesCommand, bool>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        public EditRolesHandler(IRepository repository, IMapper mapper, UserManager<User> userManager)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<bool> Handle(EditRolesCommand request, CancellationToken cancellationToken)
        {
            string id = request.Id;
            string[] roles = request.Roles;
            bool isSuccessful = false;

            User user = await repository.GetByIdAsync<User>(id);

            ArgumentNullException.ThrowIfNull(user, ErrorMessageConstants.INVALID_USER);

            IEnumerable<string> userRoles = await userManager.GetRolesAsync(user);

            await userManager.RemoveFromRolesAsync(user, userRoles);

            if (roles.Length > 0)
            {
                await userManager.AddToRolesAsync(user, roles);
            }

            isSuccessful = true;

            return isSuccessful;
        }
    }
}

