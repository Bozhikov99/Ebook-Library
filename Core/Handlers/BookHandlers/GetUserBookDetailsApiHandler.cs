using AutoMapper;
using Core.ApiModels.InputModels.Books;
using Core.Helpers;
using Core.Queries.Book;
using Core.ViewModels.Review;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Handlers.BookHandlers
{
    public class GetUserBookDetailsApiHandler : IRequestHandler<GetUserBookDetailsApiQuery, BookDetailsApiModel>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GetUserBookDetailsApiHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<BookDetailsApiModel> Handle(GetUserBookDetailsApiQuery request, CancellationToken cancellationToken)
        {
            string bookId = request.BookId;
            string userId = request.UserId;

            Book book = await repository.AllReadonly<Book>(b => b.Id == bookId)
                .Include(b => b.Genres)
                .Include(b => b.Author)
                .Include(b => b.Reviews)
                .FirstAsync();


            BookDetailsApiModel model = mapper.Map<BookDetailsApiModel>(book);

            if (string.IsNullOrEmpty(userId))
            {
                return model;
            }

            User user = await repository.GetByIdAsync<User>(userId);

            bool isFavourite = book.UsersFavourited
                .Any(u => u.Id == userId);

            Review userReview = await repository.AllReadonly<Review>(r => r.UserId == userId)
                .FirstOrDefaultAsync();

            UserReviewModel userReviewModel = mapper.Map<UserReviewModel>(userReview);

            model.UserReview = userReviewModel;
            model.IsFavourite = isFavourite;
            model.Reviews = model.Reviews
                .Where(r => r.UserName != user.UserName);

            return model;
        }
    }
}
