using Core.ViewModels.Book;
using Core.ViewModels.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ApiModels.User
{
    public class ProfileModel
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTime RegisterDate { get; set; }

        public IEnumerable<ListBookModel> Books { get; set; }

        public ListSubscriptionModel Subscription { get; set; }
    }
}
