using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library.Presentation.MVC.Accounts
{
    public class AccountContext : IdentityDbContext<UserAccount>
    {
        public AccountContext(DbContextOptions<AccountContext> options) : base(options)
        {
        }
    }
}