using System;
using MediatR;

namespace Core.Commands.UserCommands
{
    public class EditRolesCommand: IRequest<bool>
    {
        public EditRolesCommand(string id, string[] roles)
        {
            Id = id;
            Roles = roles;
        }

        public string Id { get; set; }
        public string[] Roles { get; set; }
    }
}

