using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library.IdentityService.Models
{
    public class AccountContext : IdentityDbContext<UserAccount>
    {
        public AccountContext(DbContextOptions<AccountContext> options) : base(options)
        {
        }
    }
}