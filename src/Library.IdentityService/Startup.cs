using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.AspNetIdentity;
using Library.IdentityService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Library.IdentityService
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
            var connectionString = _configuration.GetConnectionString("AccountsDBConnectionString");
            services.AddDbContext<AccountContext>(cfg =>
            {
                cfg.UseSqlServer(connectionString);
            });

            services.AddIdentity<UserAccount, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireLowercase = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<AccountContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<AccountContext>();
            services.AddTransient<AccountsSeeder>();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Identity.Cookies";
                config.LoginPath = "/Auth/Login";
            });

            services.AddIdentityServer()
                .AddAspNetIdentity<UserAccount>()
                .AddInMemoryIdentityResources(Configuration.GetIdentityResources)
                .AddInMemoryClients(Configuration.GetClients(_configuration))
                .AddDeveloperSigningCredential();

            services.AddTransient<IUserClaimsPrincipalFactory<UserAccount>, ClaimsFactory>();
            
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseIdentityServer();

            app.UseMvc(configuration =>
            {
                configuration.MapRoute("Default", "{controller}/{action}/{id?}");
            });
        }
    }
}
