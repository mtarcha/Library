namespace Library.Domain.Common
{
    public interface IRepository<TEntity, in TId>
        where TEntity : Entity<TId>, IAggregateRoot
    {
        void Create(TEntity entity);

        TEntity GetById(TId id);

        void Update(TEntity entity);

        void Delete(TId id);
    }
}