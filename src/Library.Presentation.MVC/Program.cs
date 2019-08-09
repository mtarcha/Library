using Library.Infrastructure.Core;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Presentation.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            RunSeed(host);

            host.Run();
        }

        private static void RunSeed(IWebHost host)
        {
            var scoupeFactory = host.Services.GetService<IServiceScopeFactory>();
            using (var scoupe = scoupeFactory.CreateScope())
            {
                var seeders = scoupe.ServiceProvider.GetServices<IStorageSeeder>();
                foreach (var storageSeeder in seeders)
                {
                    storageSeeder.Seed();
                }
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
