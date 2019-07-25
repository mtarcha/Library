using System;
using System.Linq;

namespace Library.Domain
{
    public interface IRepository<TEntity, in TId>
    {
        void Create(TEntity entity);

        TEntity GetById(TId id);

        void Update(TEntity entity);

        void Delete(TId id);

        IQueryable<TEntity> Get(Predicate<TEntity> predicate);
    }

    public interface IBooksRepository : IRepository<Book, int>
    { }

    public interface IAuthorsRepository : IRepository<Author, int>
    { }
}