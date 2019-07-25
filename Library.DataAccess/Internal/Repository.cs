using System;
using System.Linq;
using System.Linq.Expressions;
using Library.DataAccess.Entities;
using Library.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Library.DataAccess
{
    internal class Repository<TEntity> : IRepository<TEntity, int> where TEntity : class, IEntity
    {
        private readonly LibraryContext _ctx;

        protected Repository(LibraryContext ctx)
        {
            _ctx = ctx;
        }

        public void Create(TEntity entity)
        {
            _ctx.Set<TEntity>().Add(entity);
            _ctx.SaveChanges();
        }

        public TEntity GetById(int id)
        {
            return _ctx.Set<TEntity>().Single(x => x.Id == id);
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
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