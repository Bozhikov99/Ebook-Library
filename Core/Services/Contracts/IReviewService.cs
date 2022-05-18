using Core.ViewModels.Review;
using Infrastructure.Models;

namespace Core.Services.Contracts
{
    public interface IReviewService
    {
        Task CreateReview(CreateReviewModel model);

        Task<UserReviewModel> GetUserReview(string userId, string bookId);

        Task DeleteReview(string id);

        Task<IEnumerable<ListReviewModel>> GetAll(string bookId);
    }
}
