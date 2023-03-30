using AutoMapper;
using Common.MessageConstants;
using Core.Commands.ReviewCommands;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Handlers.ReviewHandlers
{
    public class DeleteReviewApiHandler : IRequestHandler<DeleteReviewApiCommand>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public DeleteReviewApiHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteReviewApiCommand request, CancellationToken cancellationToken)
        {
            string reviewId = request.Id;
            string userId = request.UserId;

            bool isExistingUser = await repository.AnyAsync<User>(u => u.Id == userId);

            if (!isExistingUser)
            {
                throw new ArgumentNullException(ErrorMessageConstants.INVALID_USER);
            }

            Review review = await repository.GetByIdAsync<Review>(reviewId);

            if (review is null)
            {
                throw new ArgumentNullException(ErrorMessageConstants.REVIEW_NOT_FOUND);
            }

            bool isAuthor = review.UserId == userId;

            if (!isAuthor)
            {
                throw new InvalidOperationException(ErrorMessageConstants.UNAUTHORIZED_REVIEW);
            }

            await repository.DeleteAsync<Review>(reviewId);
            await repository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
