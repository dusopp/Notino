using Newtonsoft.Json;
using Notino.Application.AsyncronousConstructs;
using Notino.Application.Contracts.Persistence;
using Notino.Application.Exceptions;
using Notino.Domain;
using Notino.Persistence.HDD.Constants;
using Notino.Persistence.HDD.Repositories.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Notino.Persistence.HDD.Repositories
{
    public class DocumentRepository : BaseRepository<Document, string>, IDocumentRepository
    {
        private static readonly AsyncLock asyncLock = new AsyncLock();

        public DocumentRepository()
        :base(nameof(Document))
        {
        }

        public async Task<Document> AddDocumentWithTagsAsync(Document document, IEnumerable<string> tagNames, bool isUpdate = false)
        {          
            
            if(await ExistsAsync(document.Id))
                throw new AlreadyExistsException(nameof(Document), document.Id);

            using (await asyncLock.LockAsync())
            {
                var exists = await ExistsAsync(document.Id);
                if (!exists)
                {
                    await File
                        .AppendAllTextAsync(
                            GetFileName(document.Id), 
                            document.RawJson
                        );
                }
            }

            return document;
        }

        public async Task<string> DeleteDocumentWithTagsAsync(string id)
        {
            await DeleteByIdAsync(id);    
            
            return id;
        }

        public Task<Document> UpdateDocumentWithTagsAsync(Document documentToUpdate, IEnumerable<string> updatedDocumentTagNames)
        {
            throw new NotImplementedException();
        }
    }
}
