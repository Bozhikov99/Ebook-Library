using AutoMapper;
using Core.ApiModels.InputModels.Review;
using Core.ApiModels.OutputModels.Review;
using Core.ViewModels.Review;
using Domain.Entities;

namespace Core.Mapping
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<CreateReviewModel, Review>();

            CreateMap<ReviewInputModel, Review>();

            CreateMap<Review, UserReviewModel>();

            CreateMap<Review, UserReviewOutputModel>();

            CreateMap<Review, ListReviewModel>()
                .ForMember(d => d.UserName, s => s.MapFrom(r => r.User.UserName));
            
            CreateMap<Review, ListReviewOutputModel>()
                .ForMember(d => d.UserName, s => s.MapFrom(r => r.User.UserName));
        }
    }
}
