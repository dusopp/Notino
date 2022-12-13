using Notino.Application.Contracts.Persistence;
using System.Threading.Tasks;

namespace Notino.Persistence.MSSQL.Repositories.Common
{
    public class BaseRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        private readonly DocumentDbContext _dbContext;

        protected BaseRepository(DocumentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Exists(TKey id)
        {
            var entity = await GetById(id);
            return entity != null;
        }

        public async Task<TEntity> GetById(TKey id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }       

        public async Task Add(TEntity entity)
        {
            await _dbContext.AddAsync(entity);           
        }

        public async Task Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }
    }
}
