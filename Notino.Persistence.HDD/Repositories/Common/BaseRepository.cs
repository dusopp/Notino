using Notino.Application.Contracts.Persistence;
using Notino.Persistence.HDD.Constants;
using System;
using System.Threading.Tasks;

namespace Notino.Persistence.HDD.Repositories.Common
{
    public class BaseRepository<T, TKey> : IRepository<T, TKey> where T : class
    {
        public IUnitOfWork UnitOfWork => throw new NotImplementedException();

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

        public async Task<bool> Exists(TKey id)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetById(TKey id)
        {
            throw new NotImplementedException();
        }

        //toto prec
        protected string GetFileName(TKey id)
        {
            return String.Format(FileNames.Document, id); 
        }
    }
}
