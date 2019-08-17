using System.Threading;
using System.Threading.Tasks;

namespace Library.Domain.Common
{
    public interface IRepository<TEntity, in TId>
        where TEntity : Entity<TId>, IAggregateRoot
    {
        Task<TEntity> CreateAsync(TEntity entity, CancellationToken token);

        Task<TEntity> GetByIdAsync(TId id, CancellationToken token);

        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken token);

        Task DeleteAsync(TId id, CancellationToken token);
    }
}