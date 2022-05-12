using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.User
{
    public class UserProfileModel
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTime RegisterDate { get; set; }
    }
}
