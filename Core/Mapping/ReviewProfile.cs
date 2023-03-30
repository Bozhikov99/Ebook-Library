using AutoMapper;
using Core.ApiModels.Review;
using Core.ViewModels.Review;
using Domain.Entities;

namespace Core.Mapping
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<CreateReviewModel, Review>();

            CreateMap<CreateReviewApiModel, Review>();

            CreateMap<Review, UserReviewModel>();

            CreateMap<Review, ListReviewModel>()
                .ForMember(d => d.UserName, s => s.MapFrom(r => r.User.UserName));
        }
    }
}
