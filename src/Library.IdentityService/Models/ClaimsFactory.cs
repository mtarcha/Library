using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Library.IdentityService.Models
{
    public class ClaimsFactory : UserClaimsPrincipalFactory<UserAccount>
    {
        private readonly AccountContext _context;
        private readonly UserManager<UserAccount> _userManager;

        public ClaimsFactory(
            UserManager<UserAccount> userManager,
            IOptions<IdentityOptions> optionsAccessor,
            AccountContext context) : base(userManager, optionsAccessor)
        {
            _context = context;
            _userManager = userManager;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(UserAccount user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            identity.AddClaims(roles.Select(role => new Claim(JwtClaimTypes.Role, role)));

            return identity;
        }
    }
}