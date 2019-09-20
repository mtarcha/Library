using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Library.Domain;
using Library.Domain.Common;
using Library.Domain.Entities;
using Library.Domain.Repositories;
using Library.Infrastructure.Data.Internal;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Data
{
    public class UsersRepository : IUsersRepository
    {
        private readonly LibraryContext _ctx;
        private readonly IEntityFactory _entityFactory;

        public UsersRepository(LibraryContext ctx, IEntityFactory entityFactory)
        {
            _ctx = ctx;
            _entityFactory = entityFactory;
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
            await _ctx.AddAsync(userEntity, token);
           
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
                //todo: update entity properties
                return user;
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

                _ctx.Users.Remove(entity);
            }
            catch (Exception e)
            {
                throw new DomainException(e.Message);
            }
        }
    }
}