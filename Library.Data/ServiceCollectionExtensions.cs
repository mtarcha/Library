using Library.Data.Entities;
using Library.Data.Internal;
using Library.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Data
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEntityFramework(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<LibraryContext>(cfg =>
            {
                cfg.UseSqlServer(connectionString);
            });

            services.AddIdentity<UserEntity, IdentityRole>()
                .AddEntityFrameworkStores<LibraryContext>();

            services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
            
            services.AddTransient<DbInitializer>();
        }
    }
}