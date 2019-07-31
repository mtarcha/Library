using System;
using System.Security.Cryptography.X509Certificates;

namespace Library.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IBooksRepository Books { get; }

        IAuthorsRepository Authors { get; }

        IUsersRepository Users { get; }
    }
}