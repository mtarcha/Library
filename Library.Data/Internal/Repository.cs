using System;
using System.Linq;
using System.Linq.Expressions;
using Library.Data.Entities;
using Library.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Library.Data.Internal
{
    internal class Repository<TEntity> where TEntity : class, IEntity
    {
        private readonly LibraryContext _ctx;

        public Repository(LibraryContext ctx)
        {
            _ctx = ctx;
        }

        public void Create(TEntity entity)
        {
            _ctx.Set<TEntity>().Add(entity);
            _ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _ctx.Set<TEntity>().Single(x => x.Id == id);
            _ctx.Set<TEntity>().Remove(entity);

            _ctx.SaveChanges();
        }

        public void UpdateProperties(TEntity entity)
        {
            var entityEntry = _ctx.Set<TEntity>().Attach(entity);

            foreach (var property in entityEntry.Properties)
            {
                if (!property.Metadata.IsKeyOrForeignKey())
                {
                    property.IsModified = true;
                }
            }

            _ctx.SaveChanges();
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _ctx.Set<TEntity>().Where(predicate);
        }
    }
}