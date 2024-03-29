﻿using Notino.Application.Exceptions;
using Notino.Domain.Contracts.Persistence;
using Notino.Domain.Entities;
using Notino.Persistence.HDD.Repositories.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Notino.Persistence.HDD.Repositories
{
    public class DocumentRepository : BaseRepository<Document, string>, IDocumentRepository
    {       
        public DocumentRepository()
            :base(nameof(Document))
        {
        }

        public async Task<Document> AddDocumentWithTagsAsync(Document document, IEnumerable<string> tagNames, CancellationToken ct, bool isUpdate = false)
        {
            var exists = await ExistsAsync(document.Id, ct);
            if (exists)
                throw new AlreadyExistsException(nameof(Document), document.Id);
               
            await File.AppendAllTextAsync(
                GetFileName(document.Id), 
                document.RawJson, ct
            );

            return document;
        }

        public async Task<Document> DeleteDocumentWithTagsAsync(string id, CancellationToken ct)
        {
            await DeleteByIdAsync(id, ct);    
            
            return new Document();
        }

        public Task<Document> UpdateDocumentWithTagsAsync(Document documentToUpdate, IEnumerable<string> updatedDocumentTagNames, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
