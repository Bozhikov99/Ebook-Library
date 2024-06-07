﻿using AutoMapper;
using Core.ApiModels.OutputModels.Review;
using Core.Books.Queries.Details;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Handlers.BookHandlers
{
    public class GetUserBookDetailsApiHandler : IRequestHandler<GetDetailsQuery, BookDetailsOutputModel>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GetUserBookDetailsApiHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<BookDetailsOutputModel> Handle(GetDetailsQuery request, CancellationToken cancellationToken)
        {
            string bookId = request.BookId;
            Book book = await repository.AllReadonly<Book>(b => b.Id == bookId)
                .Include(b => b.Genres)
                .Include(b => b.Author)
                .Include(b => b.Reviews)
                .FirstAsync();


            BookDetailsOutputModel model = mapper.Map<BookDetailsOutputModel>(book);

            if (string.IsNullOrEmpty("x"))
            {
                return model;
            }

            User user = await repository.GetByIdAsync<User>("x");

            bool isFavourite = book.UsersFavourited
                .Any(u => u.Id == "x");

            Review userReview = await repository.AllReadonly<Review>(r => r.UserId == "x")
                .FirstOrDefaultAsync(cancellationToken);

            UserReviewOutputModel userReviewModel = mapper.Map<UserReviewOutputModel>(userReview);

            model.UserReview = userReviewModel;
            model.IsFavourite = isFavourite;
            model.Reviews = model.Reviews
                .Where(r => r.UserName != user.UserName);

            return model;
        }
    }
}