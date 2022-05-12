using AutoMapper;
using Common;
using Core.Services.Contracts;
using Core.ViewModels.User;
using Infrastructure.Common;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserService(
            IRepository repository,
            IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IHttpContextAccessor httpContextAccessor)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserProfileModel> GetProfile()
        {
            string id = GetUserId();
            User user = await repository.GetByIdAsync<User>(id);
            UserProfileModel profile = mapper.Map<UserProfileModel>(user);

            return profile;
        }

        public string GetUserId()
        {
            string userId = httpContextAccessor.HttpContext
                .User
                .FindFirstValue(ClaimTypes.NameIdentifier);

            List<User> users = repository.All<User>()
                .ToList();

            return userId;
        }

        public async Task Login(LoginUserModel model)
        {
            string username = model.UserName;

            User? user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                throw new ArgumentException(ErrorMessageConstants.INVALID_USER);
            }

            bool isValidPassword = await userManager.CheckPasswordAsync(user, model.Password);

            if (!isValidPassword)
            {
                throw new ArgumentException(ErrorMessageConstants.INVALID_USER);
            }

            await signInManager.SignInAsync(user, true);
        }

        public async Task<IdentityResult?> Register(RegisterUserModel model)
        {
            User user = mapper.Map<User>(model);
            user.RegisterDate = DateTime.Now;

            IdentityResult? result = await userManager.CreateAsync(user, model.Password);

            return result;
        }
    }
}
