using System;
using Core.Commands.ReviewCommands;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;

namespace Core.Handlers.ReviewHandlers
{
    public class DeleteReviewHandler : IRequestHandler<DeleteReviewCommand, bool>
    {
        private readonly IRepository repository;

        public DeleteReviewHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            bool isDeleted = false;

            string id = request.Id;

            await repository.DeleteAsync<Review>(id);
            await repository.SaveChangesAsync();

            isDeleted = true;

            return isDeleted;
        }
    }
}

