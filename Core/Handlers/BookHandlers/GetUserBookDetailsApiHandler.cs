using AutoMapper;
using Core.ApiModels.InputModels.Books;
using Core.ApiModels.OutputModels.Book;
using Core.ApiModels.OutputModels.Review;
using Core.Queries.Book;
using Core.ViewModels.Review;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Handlers.BookHandlers
{
    public class GetUserBookDetailsApiHandler : IRequestHandler<GetUserBookDetailsApiQuery, BookDetailsOutputModel>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GetUserBookDetailsApiHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<BookDetailsOutputModel> Handle(GetUserBookDetailsApiQuery request, CancellationToken cancellationToken)
        {
            string bookId = request.BookId;
            string userId = request.UserId;

            Book book = await repository.AllReadonly<Book>(b => b.Id == bookId)
                .Include(b => b.Genres)
                .Include(b => b.Author)
                .Include(b => b.Reviews)
                .FirstAsync();


            BookDetailsOutputModel model = mapper.Map<BookDetailsOutputModel>(book);

            if (string.IsNullOrEmpty(userId))
            {
                return model;
            }

            User user = await repository.GetByIdAsync<User>(userId);

            bool isFavourite = book.UsersFavourited
                .Any(u => u.Id == userId);

            Review userReview = await repository.AllReadonly<Review>(r => r.UserId == userId)
                .FirstOrDefaultAsync();

            UserReviewOutputModel userReviewModel = mapper.Map<UserReviewOutputModel>(userReview);

            model.UserReview = userReviewModel;
            model.IsFavourite = isFavourite;
            model.Reviews = model.Reviews
                .Where(r => r.UserName != user.UserName);

            return model;
        }
    }
}
