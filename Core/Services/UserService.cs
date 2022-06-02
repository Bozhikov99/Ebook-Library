using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Common.MessageConstants;
using Core.Services.Contracts;
using Core.ViewModels.Book;
using Core.ViewModels.Subscription;
using Core.ViewModels.User;
using Infrastructure.Common;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserService(
            IRepository repository,
            IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IHttpContextAccessor httpContextAccessor,
            RoleManager<IdentityRole> roleManager)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.httpContextAccessor = httpContextAccessor;
            this.roleManager = roleManager;
        }

        public async Task<IEnumerable<ListUserModel>> GetAll()
        {
            IEnumerable<ListUserModel> users = await repository.All<User>()
                .ProjectTo<ListUserModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return users;
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
            User existingUser = await repository.All<User>().FirstOrDefaultAsync(u => u.UserName == model.UserName);
            User existingEmail = await repository.All<User>().FirstOrDefaultAsync(u => u.Email == model.Email);
            if (existingUser != null)
            {
                throw new ArgumentException(ErrorMessageConstants.USER_EXISTS);
            }
            if (existingEmail != null)
            {
                throw new ArgumentException(ErrorMessageConstants.EMAIL_EXISTS);
            }
            IdentityResult? result = await userManager.CreateAsync(user, model.Password);

            return result;
        }

        public async Task AddBookToFavourites(string id)
        {
            string userId = GetUserId();
            User user = await repository.GetByIdAsync<User>(userId);
            Book book = await repository.GetByIdAsync<Book>(id);

            user.FavouriteBooks.Add(book);

            repository.Update(user);
            await repository.SaveChangesAsync();
        }

        public async Task RemoveBookFromFavourites(string id)
        {
            string userId = GetUserId();
            User user = repository.All<User>()
                .Include(u => u.FavouriteBooks)
                .First(u => u.Id == userId);

            Book book = await repository.GetByIdAsync<Book>(id);

            user.FavouriteBooks.Remove(book);

            repository.Update(user);
            await repository.SaveChangesAsync();
        }

        public async Task<bool> IsBookFavourite(string id)
        {
            string userId = GetUserId();
            User user = repository.All<User>()
                .Include(u => u.FavouriteBooks)
                .First(u => u.Id == userId);

            bool isBookFavourite = user
                .FavouriteBooks
                .Any(b => b.Id == id);

            return isBookFavourite;
        }

        public async Task<IEnumerable<ListBookModel>> GetFavouriteBooks()
        {
            string userId = GetUserId();
            User user = await repository.GetByIdAsync<User>(userId);

            IEnumerable<ListBookModel> favouriteBooks = await repository.All<Book>(b => b.UsersFavourited.Contains(user))
                .ProjectTo<ListBookModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return favouriteBooks;
        }

        public async Task<ListSubscriptionModel> GetActiveSubscription()
        {
            string userId = GetUserId();
            Subscription subscription = await repository.All<Subscription>(s => s.UserId == userId && s.Deadline > DateTime.Now)
                .FirstOrDefaultAsync();

            ListSubscriptionModel model = mapper.Map<ListSubscriptionModel>(subscription);

            return model;
        }

        public async Task<bool> isSubscribed()
        {
            string userId = GetUserId();
            Subscription subscription = await repository.All<Subscription>(s => s.UserId == userId)
                .FirstOrDefaultAsync(s => s.Deadline < DateTime.Now);

            return subscription != null;
        }

        public async Task<bool> IsAdmin()
        {
            string userId = GetUserId();

            User user = await repository.GetByIdAsync<User>(userId);
            bool isAdmin = await userManager.IsInRoleAsync(user, RoleConstants.Administrator);

            return isAdmin;
        }
    }
}
