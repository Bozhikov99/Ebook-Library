using System;
using System.Linq.Expressions;
using AutoMapper;
using Core.Queries.Review;
using Core.ViewModels.Review;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Handlers.ReviewHandlers
{
    public class GetUserReviewHandler : IRequestHandler<GetUserReviewQuery, UserReviewModel>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GetUserReviewHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<UserReviewModel> Handle(GetUserReviewQuery request, CancellationToken cancellationToken)
        {
            string userId = request.UserId;
            string bookId = request.BookId;

            Expression<Func<Review, bool>> expression =
                 r => r.UserId == userId && r.BookId == bookId;

            Review review = await repository.All(expression)
                .FirstOrDefaultAsync();

            UserReviewModel model = mapper.Map<UserReviewModel>(review);

            return model;
        }
    }
}

