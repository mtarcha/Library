using AutoMapper;
using Library.Business;
using Library.Business.EventHandling;
using Library.Domain;
using Library.Domain.Common;
using Library.Infrastucture.Data;
using Library.Presentation.MVC.EventHandlers;
using Library.Presentation.MVC.Utility;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using BookRateChangedEvent = Library.Domain.Events.BookRateChangedEvent;

namespace Library.Presentation.MVC
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfiles(new Profile[]
                {
                    new ViewModelsToDTOMapper(),
                    new DomainToDTOMapper(), 
                });
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddSignalR();
            services.AddMediatR();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IEventDispatcher, EventDispatcher>();
            services.AddScoped<IEntityFactory, EntityFactory>();
            services.AddSingleton<IIntegrationEventHandler<Business.EventHandling.BookRateChangedEvent>, BookRateChangedEventHandler>();

            var connectionString = _configuration.GetConnectionString("LibraryConnectionString");
            services.AddEntityFramework(connectionString);
            
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                .AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseNodeModules(env);
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
