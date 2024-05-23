using Core.Common.Interfaces;
using Core.ViewModels.Book;
using Core.ViewModels.Subscription;

namespace Core.ApiModels.OutputModels.User
{
    public class UserProfileOutputModel: IHypermediaResource
    {
        public string Id { get; set; } = null!;

        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTime RegisterDate { get; set; }

        public IEnumerable<ListBookModel> Books { get; set; }

        public ListSubscriptionModel Subscription { get; set; }

        public IEnumerable<ILink> Links { get; set; } = new List<ILink>();
    }
}
