using System;
using Api.Authentication;
using Api.Authentication.Interfaces;
using AutoMapper;
using Common.MessageConstants;
using Core.Authentication;
using Core.Commands.UserCommands;
using Core.ViewModels.User;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Core.Handlers.UserHandlers
{
    public class LoginHandler : IRequestHandler<LoginCommand, bool>
    {
        //private readonly IJwtProvider jwtProvider;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;

        public LoginHandler(
            //IJwtProvider jwtProvider,
            SignInManager<User> signInManager,
            UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            //this.jwtProvider = jwtProvider;
        }

        public async Task<bool> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            bool isSuccessful = false;

            LoginUserModel model = request.Model;
            string username = model.UserName;

            User? user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                throw new InvalidUserCredentialsException(ErrorMessageConstants.INVALID_USER);
            }

            bool isValidPassword = await userManager.CheckPasswordAsync(user, model.Password);

            if (!isValidPassword)
            {
                throw new InvalidUserCredentialsException(ErrorMessageConstants.INVALID_USER);
            }

            await signInManager.SignInAsync(user, true);

            //string token = jwtProvider.GenerateToken(model);

            isSuccessful = true;

            return isSuccessful;
        }
    }
}

