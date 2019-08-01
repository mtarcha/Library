using System;
using System.Linq;
using Library.Domain;
using Library.Infrastucture.Data.Entities;
using Library.Infrastucture.Data.Internal;
using Microsoft.AspNetCore.Identity;

namespace Library.Infrastucture.Data
{
    public class UsersRepository : IUsersRepository
    {
        private readonly LibraryContext _ctx;
        private readonly EntityFactory _entityFactory;
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersRepository(
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

        public void Create(User user)
        {
            var userEntity = user.ToEntity();
            var result = _userManager.CreateAsync(userEntity, user.Password).Result;
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(userEntity, user.Role.Name).Wait();
            }
            else
            {
                var errors = result.Errors.Select(x => x.Description).Aggregate((x, y) => x + " /n" + y);
                throw new Exception(errors);
            }
        }

        public void Update(User user)
        {
            var entity = _ctx.Users.Single(x => x.ReferenceId == user.Id);

            if (!string.IsNullOrEmpty(user.Password) && !string.IsNullOrEmpty(user.NewPassword))
            {
                _userManager.ChangePasswordAsync(entity, user.Password, user.NewPassword).Wait();
            }

            if (!_userManager.IsInRoleAsync(entity, user.Role.Name).Result)
            {
                _userManager.AddToRoleAsync(entity, user.Role.Name).Wait();
            }
        }

        public void Delete(Guid id)
        {
            var entity = _ctx.Users.Single(x => x.ReferenceId == id);

            _userManager.DeleteAsync(entity).Wait();
        }

        public User GetById(Guid id)
        {
            var entity = _ctx.Users.FirstOrDefault(x => x.ReferenceId == id);
            return entity?.ToUser(_entityFactory);
        }

        public User GetByName(string userName)
        {
            var entity = _ctx.Users.FirstOrDefault(x => x.UserName == userName);
            return entity?.ToUser(_entityFactory);
        }

        public void SignOut()
        {
            _signInManager.SignOutAsync().Wait();
        }

        public bool TrySignIn(string userName, string password, bool isPersistent)
        {
            var result = _signInManager.PasswordSignInAsync(userName, password, isPersistent, false).Result;
            return result.Succeeded;
        }

        public void CreateRoleIfNotExists(Role role)
        {
            var roleExist = _roleManager.RoleExistsAsync(role.Name).Result;
            if (!roleExist)
            {
                var identityResult = _roleManager.CreateAsync(new IdentityRole(role.Name)).Result;
            }
        }
    }
}