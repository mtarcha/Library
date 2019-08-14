using System;
using System.Threading;
using System.Threading.Tasks;
using Library.Domain.Common;
using Library.Domain.Entities;

namespace Library.Domain.Repositories
{
    public interface IAuthorsRepository : IRepository<Author, Guid>
    {
        Task<Author> FindAsync(string name, string surName, DateTime dateOfBirth, DateTime? dateOfDeath, CancellationToken token);
    }
}