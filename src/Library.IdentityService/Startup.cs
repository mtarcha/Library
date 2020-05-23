using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.IdentityService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Library.IdentityService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AccountContext>(config =>
            {
                config.UseInMemoryDatabase("IdentityServerMemoryDB");
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
                .AddInMemoryApiResources(Configuration.GetApis)
                .AddInMemoryClients(Configuration.GetClients)
                .AddDeveloperSigningCredential();

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

            //app.UseMvcWithDefaultRoute();
        }
    }
}
