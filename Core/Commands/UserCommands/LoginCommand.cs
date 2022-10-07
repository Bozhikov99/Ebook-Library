using System;
using Core.ViewModels.User;
using MediatR;

namespace Core.Commands.UserCommands
{
    public class LoginCommand: IRequest<bool>
    {
        public LoginCommand(LoginUserModel model)
        {
            Model = model;
        }

        public LoginUserModel Model { get; private set; }
    }
}

