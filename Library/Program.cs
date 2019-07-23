﻿using Library.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                var seeders = scoupe.ServiceProvider.GetServices<ISeeder>();
                foreach (var seeder in seeders)
                {
                    seeder.Seed().Wait();
                }
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
