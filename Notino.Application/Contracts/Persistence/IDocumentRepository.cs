using Notino.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notino.Application.Contracts.Persistence
{
    public interface IDocumentRepository : IRepository<Document,string>
    {        
        Task AddDocumentWithTagsAsync(Document document, IEnumerable<string> tagNames);

        Task UpdateDocumentWithTagsAsync(Document documentToUpdate, IEnumerable<string> updatedDocumentTagNames);

        Task DeleteDocumentWithTagsAsync(string id);
    }
}
