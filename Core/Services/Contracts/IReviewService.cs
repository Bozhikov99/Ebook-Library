using Core.ViewModels.Review;

namespace Core.Services.Contracts
{
    public interface IReviewService
    {
        Task CreateReview(CreateReviewModel model);

        Task<UserReviewModel> GetUserReview(string userId, string bookId);

        Task DeleteReview(string id);

        Task<IEnumerable<ListReviewModel>> GetAll(string userId, string bookId);
    }
}
