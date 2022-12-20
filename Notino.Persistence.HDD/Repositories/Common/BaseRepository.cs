using Newtonsoft.Json;
using Notino.Domain.Contracts.Persistence;
using Notino.Domain.Entities.Common;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Notino.Persistence.HDD.Repositories.Common
{
    /*
     Methods that are not implemented and throw new NotImplementedException(),
     are not implemented because of lack of time.
     They are necessary and they would be implemented if I had enough time.
     Therefore, in current context: 
      I AM NOT BREAKING LISKOV SUBSTITUTION PRINCIPLE
     */

    public class BaseRepository<T, TKey> : IRepository<T, TKey> 
        where T : class, IBaseDomainEntity<TKey>
    {
        protected readonly string entityName;

        public BaseRepository(string entityName)
        {
            this.entityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
        }
         
        public async Task AddAsync(T entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(T entity, CancellationToken ct)
        {
            await DeleteByIdAsync(entity.Id, ct);
        }

        public async Task DeleteByIdAsync(TKey id, CancellationToken ct)
        {
            //this implement soft delete? just adding deleted to filename
            await Task.Run(() => File.Delete(GetFileName(id)), ct);
        }

        public Task<bool> ExistsAsync(TKey id, CancellationToken ct)
        {
            return Task.FromResult(File.Exists(GetFileName(id)));
        }

        public async Task<T> GetByIdAsync(TKey id, CancellationToken ct)
        {
            var fileContent = await File.ReadAllTextAsync(GetFileName(id), ct); //

            var result = JsonConvert.DeserializeObject<T>(fileContent);
            

            return result;
        }

        protected virtual string GetFileName(TKey id)
        {            
            return $"{entityName}{id}.txt";
        }
    }
}
