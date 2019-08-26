﻿using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Library.Presentation.MVC.Clients;
using Library.Presentation.MVC.EventHandlers;
using Library.Presentation.MVC.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RestEase;

namespace Library.Presentation.MVC
{
    // todo: add MVC client
    // todo: add RabbitMQ to send notification
    // todo: authentication
    // todo: extend docker-compose settings 
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfiles(new Profile[]
                {
                    new ViewModelsMapper(),
                    //new DomainEventsMapping(), 
                });
            });

            IMapper mapper = mappingConfig.CreateMapper();
            var apiUrl = _configuration["ApiUrl"];
            services.AddTransient(x => RestClient.For<IBooksClient>(apiUrl));
            services.AddSingleton(mapper);
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
                configuration.MapRoute("Default", "{controller=Books}/{action=Get}/{id?}");
            });
        }
    }
}
