using System.Threading.Tasks;

namespace Notino.Application.Contracts.Persistence
{
    public interface IRepository<TEntity, TKey> where TEntity : class
    {   

        Task<TEntity> GetByIdAsync(TKey id);

        Task AddAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        Task DeleteByIdAsync(TKey id);

        Task<bool> ExistsAsync(TKey id);        
        
    }
}
