using Notino.Application.Contracts.Persistence;
using Notino.Domain;
using Notino.Persistence.HDD.Repositories.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Notino.Persistence.HDD.Repositories
{
    public class DocumentRepository : BaseRepository<Document, string>, IDocumentRepository
    {       

        public async Task AddDocumentWithTagsAsync(Document document, IEnumerable<string> tagNames)
        {
            await File.AppendAllTextAsync(GetFileName(document.Id), document.Value);
            
        }


        public async Task DeleteDocumentWithTagsAsync(string id)
        {           
            await Task.Run(() => File.Delete(GetFileName(id)));           
        }  
    }
}
