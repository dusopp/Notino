using Notino.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Notino.Persistence.MSSQL.Repositories.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DocumentDbContext _context;
        private IDocumentRepository _documentRepo;

        public IDocumentRepository DocumentRepository => 
            _documentRepo;

        public UnitOfWork(DocumentDbContext dbContext, IDocumentRepository documentRepository)
        {
            _context = dbContext;
            _documentRepo = documentRepository;
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {         
            await _context.SaveChangesAsync();
        }
    }
}
