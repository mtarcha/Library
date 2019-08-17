using System.Threading;
using System.Threading.Tasks;
using Library.Infrastructure;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            RunSeed(host).Wait();

            host.Run();
        }

        private static async Task RunSeed(IWebHost host)
        {
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var seeders = scope.ServiceProvider.GetServices<IStorageSeeder>();
                foreach (var storageSeeder in seeders)
                {
                    await storageSeeder.SeedAsync(CancellationToken.None);
                }
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
