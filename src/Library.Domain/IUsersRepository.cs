using System;
using Library.Domain.Common;

namespace Library.Domain
{
    public interface IUsersRepository : IRepository<User, Guid>
    {
        User GetByName(string userName);

        void SignOut();

        bool TrySignIn(string userName, string password, bool isPersistent);

        void CreateRoleIfNotExists(Role role);
    }
}