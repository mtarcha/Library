using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Library.Domain.Common;
using Library.Domain.Entities;

namespace Library.Domain.Repositories
{
    public interface IUsersRepository : IRepository<User, Guid>
    {
        Task<IEnumerable<User>> GetFollowersAsync(Guid userId, CancellationToken token);
    }
}