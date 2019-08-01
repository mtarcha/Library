using Library.Domain;
using Library.Infrastucture.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Library.Infrastucture.Data
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly LibraryContext _ctx;
        private readonly EntityFactory _entityFactory;
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UnitOfWorkFactory(
            LibraryContext ctx, 
            EntityFactory entityFactory, 
            UserManager<UserEntity> userManager, 
            SignInManager<UserEntity> signInManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _ctx = ctx;
            _entityFactory = entityFactory;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork(_ctx, _entityFactory, _userManager, _signInManager, _roleManager);
        }
    }
}