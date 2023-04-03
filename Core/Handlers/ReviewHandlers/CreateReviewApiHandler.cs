using AutoMapper;
using Common.MessageConstants;
using Core.ApiModels.InputModels.Review;
using Core.Commands.ReviewCommands;
using Core.ViewModels.Book;
using Core.ViewModels.Review;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Core.Handlers.ReviewHandlers
{
    public class CreateReviewApiHandler : IRequestHandler<CreateReviewApiCommand, ListReviewModel>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public CreateReviewApiHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<ListReviewModel> Handle(CreateReviewApiCommand request, CancellationToken cancellationToken)
        {
            string bookId = request.BookId;
            ReviewInputModel model = request.Model;

            bool isExistingBook = await repository.AnyAsync<Book>(b => b.Id == bookId);

            if (!isExistingBook)
            {
                throw new ArgumentNullException(ErrorMessageConstants.BOOK_DOES_NOT_EXIST);
            }

            Expression<Func<Review, bool>> usersReviewExpression = r => r.BookId == bookId && r.UserId == model.UserId;

            Review existingReview = repository.AllReadonly(usersReviewExpression)
                .FirstOrDefault();

            if (existingReview is not null)
            {
                await repository.DeleteAsync<Review>(existingReview.Id);
            }

            Review review = mapper.Map<Review>(model);
            review.BookId = bookId;

            await repository.AddAsync(review);
            await repository.SaveChangesAsync();

            Review createdReview = await repository.AllReadonly(usersReviewExpression)
                .Include(r => r.User)
                .FirstAsync();

            ListReviewModel outputModel = mapper.Map<ListReviewModel>(createdReview);

            return outputModel;
        }
    }
}
