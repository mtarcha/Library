using System;
using Library.Domain.Repositories;

namespace Library.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IBooksRepository Books { get; }

        IAuthorsRepository Authors { get; }

        IUsersRepository Users { get; }
    }
}