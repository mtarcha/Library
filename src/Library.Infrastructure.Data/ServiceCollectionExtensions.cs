using Library.Domain;
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

            services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
            
            services.AddTransient<IStorageSeeder, DbInitializer>();
        }
    }
}