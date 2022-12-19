using System.Threading;
using System.Threading.Tasks;

namespace Notino.Application.Contracts.Persistence
{
    public interface IRepository<TEntity, TKey> where TEntity : class
    {   

        Task<TEntity> GetByIdAsync(TKey id, CancellationToken ct);

        Task AddAsync(TEntity entity, CancellationToken ct);

        Task DeleteAsync(TEntity entity, CancellationToken ct);

        Task DeleteByIdAsync(TKey id, CancellationToken ct);

        Task<bool> ExistsAsync(TKey id, CancellationToken ct);        
        
    }
}
