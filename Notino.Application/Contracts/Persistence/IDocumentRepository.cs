﻿using Notino.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Notino.Application.Contracts.Persistence
{
    public interface IDocumentRepository : IRepository<Document, string>
    {        
        Task<Document> AddDocumentWithTagsAsync(
            Document document, 
            IEnumerable<string> tagNames,
            CancellationToken ct,
            bool isUpdate = false);

        Task<Document> UpdateDocumentWithTagsAsync(Document documentToUpdate, IEnumerable<string> updatedDocumentTagNames, CancellationToken ct);

        Task<string> DeleteDocumentWithTagsAsync(string id, CancellationToken ct);
    }
}
