using AutoMapper;
using Library.Application.EventHandling;
using Library.Application.EventHandling.Events;
using Library.Application.EventHandling.Handlers;
using Library.Application.Queries.Sql;
using Library.Domain;
using Library.Domain.Common;
using Library.Infrastructure.Data;
using Library.Presentation.MVC.EventHandlers;
using Library.Presentation.MVC.Utility;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

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
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfiles(new Profile[]
                {
                    new ViewModelsMapper(),
                    new DomainEventsMapping(), 
                });
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddSignalR();
            services.AddMediatR();
           
            services.AddScoped<IEventDispatcher, EventDispatcher>();
            services.AddScoped<IEntityFactory, EntityFactory>();
            services.AddSingleton<IIntegrationEventHandler<BookRateChangedEvent>, BookRateChangedEventHandler>();
            var connectionString = _configuration.GetConnectionString("LibraryConnectionString");
            services.AddEntityFramework(connectionString);
            services.AddSingleton<IConnectionFactory>(new ConnectionFactory(connectionString));
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

            app.UseStaticFiles();
            app.UseAuthentication();

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
