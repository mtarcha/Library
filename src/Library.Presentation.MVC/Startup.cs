using System;
using AutoMapper;
using Library.Infrastructure.Messaging.RabbitMq;
using Library.Presentation.MVC.Accounts;
using Library.Presentation.MVC.Clients;
using Library.Presentation.MVC.EventHandlers;
using Library.Presentation.MVC.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RestEase;

namespace Library.Presentation.MVC
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //todo: delete
            var connectionString = _configuration.GetConnectionString("AccountsDBConnectionString");
            services.AddDbContext<AccountContext>(cfg =>
                            {
                                cfg.UseSqlServer(connectionString);
                            });

            services.AddIdentity<UserAccount, IdentityRole>().AddEntityFrameworkStores<AccountContext>();
            
            var identityServiceUrl = _configuration["IdentityServiceUrl"];

            services.AddAuthentication(config =>
                {
                    config.DefaultScheme = "Cookie";
                    config.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookie")
                .AddOpenIdConnect("oidc", config =>
                {
                    config.ClientId = "my_client1_id";
                    config.ClientSecret = "my_client1_secret";
                    config.Authority = identityServiceUrl;
                    config.SaveTokens = true;
                    config.ResponseType = "code";
                });

            services.AddTransient<AccountContext>();
            services.AddTransient<AccountsSeeder>();

            var rabbitMqConnectionString = _configuration["RabbitMqConnectionString"];
            services.AddRabbitMq(rabbitMqConnectionString);

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfiles(new Profile[]
                {
                    new ViewModelsMapper(),
                });
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            var apiUrl = _configuration["ApiUrl"];
            services
                .AddHttpClient("books", c => { c.BaseAddress = new Uri(apiUrl); })
                .AddTypedClient(c => RestClient.For<IBooksClient>(c));
            
            services.AddSignalR();
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                .AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
           
            app.UseStaticFiles();

            app.UseSignalR(routes =>
            {
                routes.MapHub<BookEventsRHub>("/book-events");
            });

            app.UseMvc(configuration =>
            {
                configuration.MapRoute("Default", "{controller=Books}/{action=Search}/{id?}");
            });
        }
    }
}
