using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Domain
{
    public interface IRepository<TEntity, in TId>
        where TEntity : Entity<TId>, IAggregateRoot
    {
        void Create(TEntity entity);

        TEntity GetById(TId id);

        void Update(TEntity entity);

        void Delete(TId id);
    }

    public interface IBooksRepository : IRepository<Book, Guid>
    {
        int GetCount(Predicate<Book> predicate);

        IEnumerable<Book> Get(Predicate<Book> predicate, int skipCount, int takeCount);
    }

    public interface IAuthorsRepository : IRepository<Author, Guid>
    {
        IEnumerable<Author> GetByName(string firstName, string lastName);
    }
}