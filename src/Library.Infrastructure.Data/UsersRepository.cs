using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Library.Domain;
using Library.Domain.Common;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using Library.Infrastructure.Data.Entities;
using Library.Infrastructure.Data.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Data
{
    public class UsersRepository : IUsersRepository
    {
        private readonly LibraryContext _ctx;
        private readonly IEntityFactory _entityFactory;
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public UsersRepository(
            LibraryContext ctx,
            IEntityFactory entityFactory, 
            UserManager<UserEntity> userManager, 
            SignInManager<UserEntity> signInManager, 
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            _ctx = ctx;
            _entityFactory = entityFactory;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<User> GetByNameAsync(string userName, CancellationToken token)
        {
            var entity = await _ctx.Users
                .Include(x => x.FavoriteBooks)
                .Include(x => x.FavoriteReviewers)
                .Include(x => x.RecommendedToRead)
                .SingleOrDefaultAsync(x => x.UserName == userName, token);

            return entity?.ToUser(_entityFactory);
        }

        public async Task LogoutAsync(CancellationToken token)
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<User> LoginAsync(string userName, string password, bool isPersistent, CancellationToken token)
        {
            var result = await _signInManager.PasswordSignInAsync(userName, password, isPersistent, false);

            if (!result.Succeeded)
            {
                throw new DomainException($"Login result: {result}.");
            }

            return await GetByNameAsync(userName, token);
        }

        public async Task CreateRoleIfNotExistsAsync(Role role, CancellationToken token)
        {
            var roleExist = await _roleManager.RoleExistsAsync(role.Name);
            if (!roleExist)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole<Guid>(role.Name));

                if(!result.Succeeded)
                {
                    throw new DomainException(string.Join("\n", result.Errors.Select(x => x.Description)));
                }
            }
        }

        public async Task<IEnumerable<User>> GetFollowersAsync(Guid userId, CancellationToken token)
        {
            var entity = await _ctx.Users.SingleAsync(x => x.Id == userId, token);
            var followers = _ctx.Users.Where(x => x.FavoriteReviewers.Contains(entity)).ToList();
            
            return followers.Select(x => x.ToUser(_entityFactory));
        }

        public async Task<User> CreateAsync(User user, CancellationToken token)
        {
            var userEntity = user.ToEntity();
            var result = await _userManager.CreateAsync(userEntity, user.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(userEntity, user.Role.Name);
            }
            else
            {
                throw new DomainException(string.Join("\n", result.Errors.Select(x => x.Description)));
            }

            return userEntity.ToUser(_entityFactory);
        }

        public async Task<User> GetByIdAsync(Guid id, CancellationToken token)
        {
            var entity = await _ctx.Users
                .Include(x => x.FavoriteBooks)
                .Include(x => x.FavoriteReviewers)
                .Include(x => x.RecommendedToRead)
                .FirstOrDefaultAsync(x => x.Id == id, token);

            return entity?.ToUser(_entityFactory);
        }

        public async Task<User> UpdateAsync(User user, CancellationToken token)
        {
            try
            {
                var entity = await _ctx.Users.SingleAsync(x => x.Id == user.Id, cancellationToken: token);

                if (!string.IsNullOrEmpty(user.Password) && !string.IsNullOrEmpty(user.NewPassword))
                {
                    var result = await _userManager.ChangePasswordAsync(entity, user.Password, user.NewPassword);
                    if (!result.Succeeded)
                    {
                        throw new DomainException(string.Join("\n", result.Errors.Select(x => x.Description)));
                    }
                }

                if (!_userManager.IsInRoleAsync(entity, user.Role.Name).Result)
                {
                    var result = await _userManager.AddToRoleAsync(entity, user.Role.Name);
                    if (!result.Succeeded)
                    {
                        throw new DomainException(string.Join("\n", result.Errors.Select(x => x.Description)));
                    }
                }

                return entity.ToUser(_entityFactory);
            }
            catch (Exception e)
            {
                throw new DomainException(e.Message);
            }
        }

        public async Task DeleteAsync(Guid id, CancellationToken token)
        {
            try
            {
                var entity = await _ctx.Users.SingleAsync(x => x.Id == id, token);

                await _userManager.DeleteAsync(entity);
            }
            catch (Exception e)
            {
                throw new DomainException(e.Message);
            }
        }
    }
}