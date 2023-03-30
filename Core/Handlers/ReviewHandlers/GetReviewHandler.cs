using AutoMapper;
using Common.MessageConstants;
using Core.Queries.Review;
using Core.ViewModels.Review;
using Domain.Entities;
using Infrastructure.Common;
using MediatR;

namespace Core.Handlers.ReviewHandlers
{
    public class GetReviewHandler : IRequestHandler<GetReviewQuery, ListReviewModel>
    {
        private readonly IRepository repository;
        private readonly IMapper mapper;

        public GetReviewHandler(IRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<ListReviewModel> Handle(GetReviewQuery request, CancellationToken cancellationToken)
        {
            string id = request.Id;

            bool isExistingReview = await repository.AnyAsync<Review>(r => r.Id==id);

            if (!isExistingReview)
            {
                throw new ArgumentNullException(ErrorMessageConstants.REVIEW_NOT_FOUND);
            }

            Review entity = await repository.GetByIdAsync<Review>(id);
            ListReviewModel model = mapper.Map<ListReviewModel>(entity);

            return model;
        }
    }
}
