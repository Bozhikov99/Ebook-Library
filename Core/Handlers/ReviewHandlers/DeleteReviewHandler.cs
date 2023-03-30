using System;
using Core.Commands.ReviewCommands;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;

namespace Core.Handlers.ReviewHandlers
{
    public class DeleteReviewHandler : IRequestHandler<DeleteReviewCommand>
    {
        private readonly IRepository repository;

        public DeleteReviewHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Unit> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            string id = request.Id;

            await repository.DeleteAsync<Review>(id);
            await repository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}

