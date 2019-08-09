using System;
using Library.Domain;
using Library.Infrastructure.Core;
using Library.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure.Data
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEntityFramework(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<LibraryContext>(cfg =>
            {
                cfg.UseSqlServer(connectionString);
            });

            services.AddIdentity<UserEntity, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<LibraryContext>();

            services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
            
            services.AddTransient<IStorageSeeder, DbInitializer>();
        }
    }
}