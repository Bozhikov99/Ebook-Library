using AutoMapper;
using Common.MessageConstants;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Identity;

namespace Core.Users.Commands.Register
{
    public class RegisterCommand : IRequest<User>
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string ConfirmPassword { get; set; } = null!;

        public string Email { get; set; } = null!;
    }

    public class RegisterHandler : IRequestHandler<RegisterCommand, User>
    {
        private readonly EbookDbContext context;
        private readonly UserManager<User> userManager;

        public RegisterHandler(EbookDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<User> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            string username = request.Username;
            string email = request.Email;

            User user = new User
            {
                UserName = username,
                Email = email
            };

            user.RegisterDate = DateTime.Now;

            bool isExistingName = await context.Users
                .AnyAsync(u => string.Equals(u.UserName, username));

            bool isExistingEmail = await context.Users
                .AnyAsync(u => string.Equals(u.Email, email));

            if (isExistingName)
            {
                throw new ExistingUserRegisterException(ErrorMessageConstants.USER_EXISTS);
            }
            if (isExistingEmail)
            {
                throw new ExistingUserRegisterException(ErrorMessageConstants.EMAIL_EXISTS);
            }

            IdentityResult? result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException();
            }

            return user;
        }
    }
}

