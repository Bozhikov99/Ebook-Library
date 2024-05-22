﻿using Core;
using Core.Helpers;
using Core.Mapping;
using Infrastructure.Common;
using Infrastructure.Persistance;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Extenstions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>();
            services.AddTransient<UserIdHelper>();

            return services;
        }

        public static IServiceCollection AddMediatrFull(this IServiceCollection services)
        {
            services.AddMediatR(typeof(MediatREntryPoint).Assembly);

            return services;
        }

        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration config)
        {
            string connectionString = config.GetConnectionString("DefaultConnection");

            services.AddDbContext<EbookDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }

        public static IServiceCollection AddAutomapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => cfg.AddProfile<GenreProfile>());
            services.AddAutoMapper(cfg => cfg.AddProfile<AuthorProfile>());
            services.AddAutoMapper(cfg => cfg.AddProfile<BookProfile>());
            services.AddAutoMapper(cfg => cfg.AddProfile<UserProfile>());
            services.AddAutoMapper(cfg => cfg.AddProfile<SubscriptionProfile>());
            services.AddAutoMapper(cfg => cfg.AddProfile<ReviewProfile>());

            return services;
        }
    }
}

