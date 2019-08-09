using System;
using System.Collections.Generic;
using Library.Domain.Common;

namespace Library.Domain
{
    public interface IUsersRepository : IRepository<User, Guid>
    {
        User GetByName(string userName);

        void SignOut();

        bool TrySignIn(string userName, string password, bool isPersistent);

        void CreateRoleIfNotExists(Role role);

        IEnumerable<User> GetFollowers(Guid userId);
    }
}