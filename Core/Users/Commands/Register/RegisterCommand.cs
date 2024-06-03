using AutoMapper;
using Common.MessageConstants;
using Core.ViewModels.User;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Common;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Identity;

namespace Core.Users.Commands.Register
{
    public class RegisterCommand : IRequest<User>
    {
        public RegisterCommand(RegisterUserModel model)
        {
            Model = model;
        }

        public RegisterUserModel Model { get; private set; }
    }

    public class RegisterHandler : IRequestHandler<RegisterCommand, User>
    {
        private readonly EbookDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        public RegisterHandler(EbookDbContext context, IMapper mapper, UserManager<User> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<User> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            RegisterUserModel model = request.Model;

            User user = mapper.Map<User>(model);
            user.RegisterDate = DateTime.Now;

            bool isExistingName = await context.Users
                .AnyAsync(u => u.UserName == model.UserName);

            bool isExistingEmail = await context.Users
                .AnyAsync(u => u.Email == model.Email);

            if (isExistingName)
            {
                throw new ExistingUserRegisterException(ErrorMessageConstants.USER_EXISTS);
            }
            if (isExistingEmail)
            {
                throw new ExistingUserRegisterException(ErrorMessageConstants.EMAIL_EXISTS);
            }

            IdentityResult? result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException();
            }

            return user;
        }
    }
}

