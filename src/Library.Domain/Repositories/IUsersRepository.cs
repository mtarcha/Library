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
        Task<User> GetByNameAsync(string userName, CancellationToken token);

        Task LogoutAsync(CancellationToken token);

        Task LoginAsync(string userName, string password, bool isPersistent, CancellationToken token);

        Task CreateRoleIfNotExistsAsync(Role role, CancellationToken token);

        Task<IEnumerable<User>> GetFollowersAsync(Guid userId, CancellationToken token);
    }
}