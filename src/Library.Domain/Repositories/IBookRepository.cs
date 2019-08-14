using System;
using Library.Domain.Common;
using Library.Domain.Entities;

namespace Library.Domain.Repositories
{
    public interface IBooksRepository : IRepository<Book, Guid>
    {
    }
}