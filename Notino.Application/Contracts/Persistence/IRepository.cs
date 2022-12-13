using System.Threading.Tasks;

namespace Notino.Application.Contracts.Persistence
{
    public interface IRepository<TEntity, TKey> where TEntity : class
    {      

        Task<TEntity> GetById(TKey id);

        Task Add(TEntity entity);

        Task Delete(TEntity entity);

        Task<bool> Exists(TKey id);
        
    }
}
