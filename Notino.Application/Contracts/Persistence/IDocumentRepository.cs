using Notino.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notino.Application.Contracts.Persistence
{
    public interface IDocumentRepository : IRepository<Document, string>
    {        
        Task<Document> AddDocumentWithTagsAsync(
            Document document, 
            IEnumerable<string> tagNames, 
            bool isUpdate = false);

        Task<Document> UpdateDocumentWithTagsAsync(Document documentToUpdate, IEnumerable<string> updatedDocumentTagNames);

        Task<string> DeleteDocumentWithTagsAsync(string id);
    }
}
