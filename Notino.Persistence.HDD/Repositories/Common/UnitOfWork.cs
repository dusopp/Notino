using Notino.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Notino.Persistence.HDD.Repositories.Common
{
    public class UnitOfWork : IUnitOfWork
    {
       
        private IDocumentRepository _documentRepo;

        public IDocumentRepository DocumentRepository =>
            _documentRepo;

        public UnitOfWork(IDocumentRepository documentRepository)
        {           
            _documentRepo = documentRepository;
        }

        public async Task SaveAsync()
        {
            
        }
    }
}
