using System;
using AutoMapper;
using Core.Commands.ReviewCommands;
using Core.ViewModels.Review;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Handlers.ReviewHandlers
{
    public class CreateReviewHandler : IRequestHandler<CreateReviewCommand, bool>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public CreateReviewHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<bool> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            bool isCreated = false;

            CreateReviewModel model = request.Model;

            string userId = model.UserId;
            string bookId = model.BookId;

            Review existingReview = await repository.AllReadonly<Review>(r => r.UserId == userId && r.BookId == bookId)
                .FirstOrDefaultAsync();

            if (existingReview != null)
            {
                await repository.DeleteAsync<Review>(existingReview.Id);
            }

            Review review = mapper.Map<Review>(model);

            await repository.AddAsync(review);
            await repository.SaveChangesAsync();

            isCreated = true;

            return isCreated;
        }
    }
}

