using System;
using System.Collections.Generic;
using Library.Domain.Common;

namespace Library.Domain
{
    public interface IBooksRepository : IRepository<Book, Guid>
    {
        int GetCount(string searchPatter);

        IEnumerable<Book> Get(string searchPatter, int skipCount, int takeCount);
    }
}