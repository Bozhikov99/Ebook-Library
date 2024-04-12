using AutoMapper;
using Core.ApiModels.OutputModels.Review;
using Core.Helpers;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Books.Queries.Details
{
    public class GetDetailsQuery : IRequest<BookDetailsOutputModel>
    {
        public GetDetailsQuery(string bookId)
        {
            BookId = bookId;
        }

        public string BookId { get; private set; }
    }

    public class GetUserBookDetailsApiHandler : IRequestHandler<GetDetailsQuery, BookDetailsOutputModel>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;
        private readonly UserIdHelper userIdHelper;

        public GetUserBookDetailsApiHandler(IRepository repository, IMapper mapper, UserIdHelper userIdHelper)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.userIdHelper = userIdHelper;
        }

        public async Task<BookDetailsOutputModel> Handle(GetDetailsQuery request, CancellationToken cancellationToken)
        {
            string bookId = request.BookId;
            string userId = userIdHelper.GetUserId();

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

            Review? userReview = await repository.AllReadonly<Review>(r => r.UserId == userId)
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
