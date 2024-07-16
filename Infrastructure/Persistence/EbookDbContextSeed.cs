using Common;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructure.Persistance
{
    public static class EbookDbContextSeed
    {
        public static async Task SeedAdministratorAsync(AccountSettings settings, UserManager<User> userManager)
        {
            User user = new User
            {
                Id = Guid.NewGuid()
                    .ToString(),
                RegisterDate = DateTime.Now,
                UserName = settings.Username,
                EmailConfirmed = true,
                Email = settings.Email
            };

            bool isExistingUser = await userManager.Users
                .AnyAsync(u => string.Equals(u.UserName, user.UserName));

            if (!isExistingUser)
            {
                await userManager.CreateAsync(user, settings.Password);
                await userManager.AddToRoleAsync(user, RoleConstants.Administrator);
            }
        }

        //TODO:
        //public static async Task SeedDefaultUserAsync()
        //{

        //}

        public static async Task SeedAdministratorRoleAsync(RoleManager<IdentityRole> roleManager)
        {
            bool isExistingRole = await roleManager.Roles
                .AnyAsync(r => string.Equals(r.Name, RoleConstants.Administrator));

            if (!isExistingRole)
            {
                await roleManager.CreateAsync(new IdentityRole(RoleConstants.Administrator));
            }
        }
    }
}
