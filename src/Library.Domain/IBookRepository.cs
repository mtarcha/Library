using System;
using System.Collections.Generic;
using Library.Domain.Common;

namespace Library.Domain
{
    public interface IBooksRepository : IRepository<Book, Guid>
    {
        int GetCount(Predicate<Book> predicate);

        IEnumerable<Book> Get(Predicate<Book> predicate, int skipCount, int takeCount);
    }
}