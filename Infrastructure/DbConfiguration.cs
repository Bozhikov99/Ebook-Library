using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure
{
    public class DbConfiguration : IdentityDbContext<User>
    {
        public const string ConnectionString = "Server=127.0.0.1,1434;Database=EBookLibrary;User=sa;Password=Docker*Database*2022;";
    }
}
