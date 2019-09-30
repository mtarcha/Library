using System;
using AutoMapper;
using EasyCaching.Core;
using EasyCaching.Core.Configurations;
using EasyCaching.Redis;
using FluentValidation.AspNetCore;
using Library.Api.Middlewares;
using Library.Api.Utility;
using Library.Application.EventHandling;
using Library.Application.EventHandling.Events;
using Library.Application.EventHandling.Handlers;
using Library.Application.Queries.Sql;
using Library.Domain;
using Library.Domain.Common;
using Library.Infrastructure;
using Library.Infrastructure.Cache;
using Library.Infrastructure.Cache.Redis;
using Library.Infrastructure.Data;
using Library.Infrastructure.Messaging.RabbitMq;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using ExceptionHandlerMiddleware = Library.Api.Middlewares.ExceptionHandlerMiddleware;

namespace Library.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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

            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddMediatR();
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped<IEventDispatcher, EventDispatcher>();
            services.AddScoped<IEntityFactory, EntityFactory>();
            services.AddSingleton<IIntegrationEventHandler<BookRateChangedEvent>, BookRateChangedEventHandler>();
            var connectionString = Configuration.GetConnectionString("LibraryConnectionString");
            services.AddEntityFramework(connectionString);
            services.AddSingleton<IConnectionFactory>(new ConnectionFactory(connectionString));
            services.AddMvc()
                .AddFluentValidation(x => x.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<IDistributedCache, DistributedCache>();

            var rabbitMqConnectionString = Configuration["RabbitMqConnectionString"];
            services.AddRabbitMq(rabbitMqConnectionString);

            var redisHost = Configuration["RedisHost"];
            var redisPort = int.Parse(Configuration["RedisPort"]);
            services.AddEasyCaching(options =>
            {
                options.UseRedis(config =>
                {
                    config.DBConfig.Endpoints.Add(new ServerEndPoint(redisHost, redisPort));
                }, "library.api");
            });

            services
                .AddSwaggerGen(c =>
                {
                    c.DescribeAllEnumsAsStrings();

                    c.SwaggerDoc("v1", new Info
                    {
                        Version = "v1",
                        Title = "Home Library Api",
                        Contact = new Contact
                        {
                            Name = "Home Library",
                            Email = "mtarcha@outlook.com",
                            Url = "https://github.com/mtarcha/Library"
                        }
                    });
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseMiddleware<CachingMiddleware>();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Home Library V1");
            });
        }
    }
}
