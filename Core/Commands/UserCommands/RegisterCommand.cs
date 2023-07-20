using System;
using Core.ViewModels.User;
using Domain.Entities;
using MediatR;

namespace Core.Commands.UserCommands
{
    public class RegisterCommand: IRequest<User>
    {
        public RegisterCommand(RegisterUserModel model)
        {
            Model = model;
        }

        public RegisterUserModel Model { get; private set; }
    }
}

