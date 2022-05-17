using Core.ViewModels.Book;
using Core.ViewModels.Subscription;
using Core.ViewModels.User;
using Microsoft.AspNetCore.Identity;

namespace Core.Services.Contracts
{
    public interface IUserService
    {
        Task<IdentityResult?> Register(RegisterUserModel model);

        Task Login(LoginUserModel model);

        Task<UserProfileModel> GetProfile();

        Task AddBookToFavourites(string id);

        Task RemoveBookFromFavourites(string id);

        Task<bool> IsBookFavourite(string id);

        Task<IEnumerable<ListBookModel>> GetFavouriteBooks();

        Task<ListSubscriptionModel> GetActiveSubscription();

        Task<bool> isSubscribed();

        string GetUserId();
    }
}
