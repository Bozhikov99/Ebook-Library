﻿using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Persistance
{
    public class DbConfiguration : IdentityDbContext<User>
    {
        public const string ConnectionString = "Server=.;Database=EBookLibrary;Integrated Security=True;";
    }
}
