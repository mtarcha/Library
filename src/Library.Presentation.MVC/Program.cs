using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Library.Presentation.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            //RunSeed(host).Wait();

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        //private static async Task RunSeed(IWebHost host)
        //{
        //    var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
        //    using (var scope = scopeFactory.CreateScope())
        //    {
        //        var seeder = scope.ServiceProvider.GetService<AccountsSeeder>();
        //        await seeder.SeedAsync();
        //    }
        //}
    }
}
