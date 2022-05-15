using Core.ViewModels.Book;
using Core.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        string GetUserId();
    }
}
