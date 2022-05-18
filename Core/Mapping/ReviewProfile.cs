using AutoMapper;
using Core.ViewModels.Review;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
