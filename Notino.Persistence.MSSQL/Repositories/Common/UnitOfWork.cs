using Notino.Application.Contracts.Persistence;
using System;
using System.Threading.Tasks;

namespace Notino.Persistence.MSSQL.Repositories.Common
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly NotinoDbContext _context;
        private IDocumentRepository _documentRepo;

        public IDocumentRepository DocumentRepository => 
            _documentRepo;

        public UnitOfWork(NotinoDbContext dbContext, IDocumentRepository documentRepository)
        {
            _context = dbContext;
            _documentRepo = documentRepository;
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task SaveAsync()
        {         
            await _context.SaveChangesAsync();
        }
    }
}
