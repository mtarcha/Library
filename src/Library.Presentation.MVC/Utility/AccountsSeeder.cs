using System;
using System.Threading.Tasks;
using Library.Presentation.MVC.Accounts;
using Microsoft.AspNetCore.Identity;

namespace Library.Presentation.MVC.Utility
{
    public class AccountsSeeder
    {
        private readonly AccountContext _accountContext;
        private readonly UserManager<UserAccount> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountsSeeder(AccountContext userContext, UserManager<UserAccount> userManager, RoleManager<IdentityRole> roleManager)
        {
            _accountContext = userContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            await _accountContext.Database.EnsureCreatedAsync();

            var roleNames = new[] { Constants.UserRoleName, Constants.AdminRoleName };

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
                var powerUser = new UserAccount
                {
                    UserName = name,
                    PhoneNumber = "+380957515212",
                    DateOfBirth = new DateTime(1993, 5, 5)
                };

                var createPowerUser = await _userManager.CreateAsync(powerUser, "K.,k. ;bnnz1");
                if (createPowerUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(powerUser, Constants.AdminRoleName);
                }
            }
        }
    }
}