using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Lubrary.IdentityServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Lubrary.IdentityServer
{
    public class SeedData
    {
        public static async Task EnsureSeedData(IServiceProvider provider)
        {
            provider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
            provider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
            provider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();

            {
                var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();

                var roles = new[] { "User", "Admin" };
                foreach (var role in roles)
                {
                    var roleExist = await roleManager.RoleExistsAsync(role);
                    if (!roleExist)
                    {
                        var result = await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                var userMgr = provider.GetRequiredService<UserManager<IdentityUser>>();
                var myroslava = userMgr.FindByNameAsync("Myroslava_Admin").Result;
                if (myroslava == null)
                {
                    myroslava = new IdentityUser
                    {
                        UserName = "Myroslava_Admin"
                    };
                    var result = userMgr.CreateAsync(myroslava, "K.,k. ;bnnz1!").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    await userMgr.AddToRoleAsync(myroslava, "Admin");

                    result = await userMgr.AddClaimsAsync(myroslava, new Claim[]{
                                new Claim(JwtClaimTypes.Name, "Myroslava Tarcha"),
                                new Claim(JwtClaimTypes.GivenName, "Myroslava"),
                                new Claim(JwtClaimTypes.FamilyName, "Tarcha"),
                                new Claim(JwtClaimTypes.Email, "tarcham1993@gmail.com"),
                                new Claim(JwtClaimTypes.Role, "Admin")});
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                }
            }

            {
                var context = provider.GetRequiredService<ConfigurationDbContext>();
                context.Database.EnsureCreated();
                if (!context.Clients.Any())
                {
                    foreach (var client in IdentityServerConfig.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in IdentityServerConfig.GetIdentityResources())
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in IdentityServerConfig.GetApis())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}