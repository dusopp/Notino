using Notino.Application.Contracts.Persistence;
using Notino.Persistence.HDD.Constants;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Notino.Persistence.HDD.Repositories.Common
{
    public class BaseRepository<T, TKey> : IRepository<T, TKey> where T : class
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

        public Task Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task<TKey> DeleteById(TKey entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Exists(TKey id)
        {
            return Task.FromResult(File.Exists(GetFileName(id)));
        }

        public async Task<T> GetById(TKey id)
        {
            throw new NotImplementedException();
        }

        protected virtual string GetFileName(TKey id)
        {            
            return $"{entityName}{id}.txt";
        }

    }
}
