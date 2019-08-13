using System;
using Library.Domain.Common;

namespace Library.Domain
{
    public interface IBooksRepository : IRepository<Book, Guid>
    {
    }
}