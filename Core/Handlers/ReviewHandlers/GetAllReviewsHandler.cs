using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Queries.Review;
using Core.ViewModels.Review;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Handlers.ReviewHandlers
{
    public class GetAllReviewsHandler : IRequestHandler<GetAllReviewsQuery, IEnumerable<ListReviewModel>>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GetAllReviewsHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ListReviewModel>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
        {
            string userId = request.UserId;
            string bookId = request.BookId;

            IEnumerable<ListReviewModel> reviews = await repository.All<Review>(r => r.UserId != userId && r.BookId == bookId)
                .ProjectTo<ListReviewModel>(mapper.ConfigurationProvider)
                .ToArrayAsync();

            return reviews;
        }
    }
}

