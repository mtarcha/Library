using System;
using System.Collections.Generic;
using Library.Domain.Common;

namespace Library.Domain
{
    public interface IAuthorsRepository : IRepository<Author, Guid>
    {
        IEnumerable<Author> GetByName(string firstName, string lastName);
    }
}