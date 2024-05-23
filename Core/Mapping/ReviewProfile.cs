using AutoMapper;
using Core.ApiModels.OutputModels.Review;
using Core.Reviews.Common;
using Core.ViewModels.Review;
using Domain.Entities;

namespace Core.Mapping
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<CreateReviewModel, Review>();

            CreateMap<Review, UserReviewModel>();

            CreateMap<Review, UserReviewOutputModel>();

            CreateMap<BaseReviewModel, ListReviewOutputModel>();
            
            CreateMap<Review, ListReviewOutputModel>()
                .ForMember(d => d.UserName, s => s.MapFrom(r => r.User.UserName));
        }
    }
}
