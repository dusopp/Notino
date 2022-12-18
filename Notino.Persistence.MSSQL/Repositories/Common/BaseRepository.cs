using Microsoft.EntityFrameworkCore;
using Notino.Application.Contracts.Persistence;
using Notino.Domain.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Notino.Persistence.MSSQL.Repositories.Common
{
    public class BaseRepository<TEntity, TKey> : IRepository<TEntity, TKey> 
        where TEntity : class, IBaseDomainEntity<TKey>, ISoftDeletableEntity
        where TKey : class
    {
        private readonly DocumentDbContext _dbContext;

        protected BaseRepository(DocumentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ExistsAsync(TKey id)
        {
            var entity = await GetByIdAsync(id);
            return entity != null;
        }

        public async Task<TEntity> GetByIdAsync(TKey id)
        {
            return await _dbContext
                .Set<TEntity>()
                .SingleOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
        }       

        public async Task AddAsync(TEntity entity)
        {
            await _dbContext.AddAsync(entity);           
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public async Task DeleteByIdAsync(TKey id)
        {
            throw new System.NotImplementedException();
        }

    }
}
