﻿using Core.Common.Interfaces;
using Core.Users.Queries.GetProfile;

namespace Core.ViewModels.User
{
    public class UserProfileModel : IHypermediaResource
    {
        public string Id { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public DateTime RegisterDate { get; set; }

        public DateTime? SubscribedDueDate { get; set; }

        public IEnumerable<FavouriteBookDto> FavouriteBooks { get; set; } = new List<FavouriteBookDto>();

        public IEnumerable<ILink> Links { get; set; } = new List<ILink>();
    }
}
