using Infrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure
{
    public class DbConfiguration : IdentityDbContext<User>
    {
        public const string ConnectionString = @"Server=DESKTOP-P8F7PSV\SQLEXPRESS;Database=EbookLibrary;Integrated Security=True;";
        public const string ContributorConnectionString = @"Server=HEATHEN;Database=EbookLibrary;Integrated Security=True;";
    }
}
