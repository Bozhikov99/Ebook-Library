using System;
using Core.ViewModels.User;
using MediatR;

namespace Core.Commands.UserCommands
{
    public class RegisterCommand: IRequest<bool>
    {
        public RegisterCommand(RegisterUserModel model)
        {
            Model = model;
        }

        public RegisterUserModel Model { get; private set; }
    }
}

