using Microsoft.EntityFrameworkCore;
using Notino.Application.Contracts.Persistence;
using Notino.Domain.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Notino.Persistence.MSSQL.Repositories.Common
{
    public class BaseRepository<TEntity, TKey> : IRepository<TEntity, TKey> 
        where TEntity : class, IBaseDomainEntity<TKey>
        where TKey : class
    {
        private readonly NotinoDbContext _dbContext;

        protected BaseRepository(NotinoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ExistsAsync(TKey id, CancellationToken ct)
        {
            var entity = await GetByIdAsync(id, ct);
            return entity != null;
        }

        public async Task<TEntity> GetByIdAsync(TKey id, CancellationToken ct)
        {
            return await _dbContext
                .Set<TEntity>()
                .SingleOrDefaultAsync(t => t.Id == id && !t.IsDeleted, ct);
        }       

        public async Task AddAsync(TEntity entity, CancellationToken ct)
        {
            await _dbContext.AddAsync(entity, ct);   
            await _dbContext.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(TEntity entity, CancellationToken ct)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync(ct);
        }

        public async Task DeleteByIdAsync(TKey id, CancellationToken ct)
        {
            TEntity entity = await GetByIdAsync(id, ct);
            await DeleteAsync(entity, ct);
        }
    }
}
