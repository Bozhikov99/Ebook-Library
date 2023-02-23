using System;
using AutoMapper;
using Common.MessageConstants;
using Core.Commands.UserCommands;
using Core.ViewModels.User;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Core.Handlers.UserHandlers
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, bool>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        public RegisterHandler(
            IRepository repository,
            IMapper mapper,
            UserManager<User> userManager)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<bool> Handle(
            RegisterCommand request,
            CancellationToken cancellationToken)
        {
            RegisterUserModel model = request.Model;

            User user = mapper.Map<User>(model);
            user.RegisterDate = DateTime.Now;

            bool isExistingName = await repository.AnyAsync<User>(u => u.UserName == model.UserName);
            bool isExistingEmail = await repository.AnyAsync<User>(u => u.Email == model.Email);

            if (isExistingName)
            {
                throw new ExistingUserRegisterException(ErrorMessageConstants.USER_EXISTS);
            }
            if (isExistingEmail)
            {
                throw new ExistingUserRegisterException(ErrorMessageConstants.EMAIL_EXISTS);
            }

            IdentityResult? result = await userManager.CreateAsync(user, model.Password);

            return result.Succeeded;
        }
    }
}

