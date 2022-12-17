using Newtonsoft.Json;
using Notino.Application.Contracts.Persistence;
using Notino.Domain.Common;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Notino.Persistence.HDD.Repositories.Common
{
    public class BaseRepository<T, TKey> : IRepository<T, TKey> 
        where T : class, IBaseDomainEntity<TKey>
    {
        protected readonly string entityName;

        public BaseRepository(string entityName)
        {
            this.entityName = entityName;
        }
         
        public async Task Add(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(T entity)
        {
            await DeleteById(entity.Id);
        }

        public async Task DeleteById(TKey id)
        {
            await Task.Run(() => File.Delete(GetFileName(id)));
        }

        public Task<bool> Exists(TKey id)
        {
            return Task.FromResult(File.Exists(GetFileName(id)));
        }

        public async Task<T> GetById(TKey id)
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
