using Notino.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notino.Application.Contracts.Persistence
{
    public interface IDocumentRepository : IRepository<Document,string>
    {
        
        public Task<string> AddDocumentWithTagsAsync(Document document, IEnumerable<string> tagNames);
    
        public Task DeleteDocumentAsync(string id);
    }
}
