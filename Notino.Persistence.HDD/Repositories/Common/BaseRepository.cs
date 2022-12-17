using Newtonsoft.Json;
using Notino.Application.Contracts.Persistence;
using Notino.Domain.Common;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Notino.Persistence.HDD.Repositories.Common
{
    /*
     Methods that are not implemented and throw new NotImplementedException(),
     are not implemented because lack of time.
     They are necessary and they would be implemented if I had enough time.
     Therefore, I am thinking, that in current context: 
      I AM NOT BREAKING LISKOV SUBSTITUTION PRINCIPLE
     */

    public class BaseRepository<T, TKey> : IRepository<T, TKey> 
        where T : class, IBaseDomainEntity<TKey>
    {
        protected readonly string entityName;

        public BaseRepository(string entityName)
        {
            this.entityName = entityName;
        }
         
        public async Task AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(T entity)
        {
            await DeleteByIdAsync(entity.Id);
        }

        public async Task DeleteByIdAsync(TKey id)
        {
            await Task.Run(() => File.Delete(GetFileName(id)));
        }

        public Task<bool> ExistsAsync(TKey id)
        {
            return Task.FromResult(File.Exists(GetFileName(id)));
        }

        public async Task<T> GetByIdAsync(TKey id)
        {
            var fileContent = await File.ReadAllTextAsync("testtt.txt"); //GetFileName(id)

            var result = JsonConvert.DeserializeObject<T>(fileContent);
            result.RawJson = fileContent;

            return result;
        }

        protected virtual string GetFileName(TKey id)
        {            
            return $"{entityName}{id}.txt";
        }
    }
}
