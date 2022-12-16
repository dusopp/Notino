using System.Threading.Tasks;

namespace Notino.Application.Contracts.Persistence
{
    public interface IRepository<TEntity, TKey> where TEntity : class
    {   

        Task<TEntity> GetById(TKey id);

        Task Add(TEntity entity);

        Task Delete(TEntity entity);

        Task<TKey> DeleteById(TKey id);

        Task<bool> Exists(TKey id);        
        
    }
}
