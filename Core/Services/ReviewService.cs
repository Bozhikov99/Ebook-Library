using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Services.Contracts;
using Core.ViewModels.Review;
using Infrastructure.Common;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public ReviewService(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task CreateReview(CreateReviewModel model)
        {
            string userId = model.UserId;
            string bookId = model.BookId;

            UserReviewModel existingReview = await GetUserReview(userId, bookId);

            if (existingReview != null)
            {
                await repository.DeleteAsync<Review>(existingReview.Id);
            }

            Review review = mapper.Map<Review>(model);

            await repository.AddAsync(review);
            await repository.SaveChangesAsync();
        }

        public async Task DeleteReview(string id)
        {
            await repository.DeleteAsync<Review>(id);
            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ListReviewModel>> GetAll(string userId, string bookId)
        {
            IEnumerable<ListReviewModel> reviews = await repository.All<Review>(r => r.UserId != userId && r.BookId == bookId)
                .ProjectTo<ListReviewModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return reviews;
        }

        public async Task<UserReviewModel> GetUserReview(string userId, string bookId)
        {
            Expression<Func<Review, bool>> expression =
                r => r.UserId == userId && r.BookId == bookId;

            Review review = await repository.All(expression)
                .FirstOrDefaultAsync();

            UserReviewModel model = mapper.Map<UserReviewModel>(review);

            return model;
        }
    }
}
