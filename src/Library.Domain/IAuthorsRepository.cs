using System;
using Library.Domain.Common;

namespace Library.Domain
{
    public interface IAuthorsRepository : IRepository<Author, Guid>
    {
    }
}