using System;
using System.Threading.Tasks;
using Library.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Library.Data.Seeders
{
    public class RolesSeeder : ISeeder
    {
        private readonly UserContext _ctx;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesSeeder(UserContext ctx, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _ctx = ctx;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Seed()
        {
            _ctx.Database.EnsureCreated();

            string[] roleNames = { Roles.Admin, Roles.User };
            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var name = "AdminMyroslava";
            var user = await _userManager.FindByNameAsync(name);
            if (user == null)
            {

                var poweruser = new User
                {
                    UserName = name,
                    PhoneNumber = "+380957515212",
                    DateOfBirth = new DateTime(1993, 5, 5)
                };

                var createPowerUser = await _userManager.CreateAsync(poweruser, "K.,k. ;bnnz1");
                if (createPowerUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(poweruser, Roles.Admin);
                }
            }
        }
    }
}