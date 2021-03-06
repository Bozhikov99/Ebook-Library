using AutoMapper;
using Core.ViewModels.Review;
using Infrastructure.Models;

namespace Core.Mapping
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<CreateReviewModel, Review>();

            CreateMap<Review, UserReviewModel>();

            CreateMap<Review, ListReviewModel>()
                .ForMember(d => d.UserName, s => s.MapFrom(r => r.User.UserName));
        }
    }
}
