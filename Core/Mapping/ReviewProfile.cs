using AutoMapper;
using Core.ApiModels.OutputModels.Review;
using Domain.Entities;

namespace Core.Mapping
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, UserReviewOutputModel>();
        }
    }
}
