using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Library.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Library
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
                var lubrarySeeder = scoupe.ServiceProvider.GetService<LibrarySeeder>();
                lubrarySeeder.Seed();

                var userSeeder = scoupe.ServiceProvider.GetService<UserSeeder>();
                userSeeder.Seed();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(AddConfigurations)
                .UseStartup<Startup>();

        private static void AddConfigurations(WebHostBuilderContext arg1, IConfigurationBuilder builder)
        {
            builder.Sources.Clear();

            builder.AddJsonFile("config.json");
        }
    }
}
