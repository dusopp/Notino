using Microsoft.EntityFrameworkCore;
using Notino.Application.Contracts.Persistence;
using Notino.Application.Exceptions;
using Notino.Domain.Entities;
using Notino.Persistence.MSSQL.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Notino.Persistence.MSSQL.Repositories
{
    public class DocumentRepository: BaseRepository<Document, string>, IDocumentRepository
    {
        private readonly NotinoDbContext _dbContext;

        public DocumentRepository(NotinoDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }       

        public async Task<Document> AddDocumentWithTagsAsync
            (Document document, 
            IEnumerable<string> documentTags,
            CancellationToken ct,
            bool isUpdate = false)
        {
            var storedDocument = await GetByIdAsync(document.Id, ct);

            if (storedDocument != null && !isUpdate)
                throw new AlreadyExistsException(nameof(Document), document.Id);

            await ManageDocumentTags(document, documentTags, ct);

            await _dbContext.Documents.AddAsync(document, ct);  

            await _dbContext.SaveChangesAsync(ct);
            
            return document;
        }

        private async Task<List<Tag>> GetStoredTags(IEnumerable<string> tagNames, CancellationToken ct)
        {
           return await _dbContext.Tags
                            .Where(x => tagNames.Contains(x.Name))
                            .Distinct()
                            .ToListAsync(ct);
        }

        private IEnumerable<Tag> GetNotStoredTags(
            IEnumerable<string> documentTagNames, 
            IEnumerable<Tag> storedTags)
        {
            return documentTagNames
                .Where(c => !storedTags.Any(x => x.Name == c))
                .Select(tagName => new Tag { Name = tagName })
                .ToList();
        }

        public async Task<string> DeleteDocumentWithTagsAsync(string id, CancellationToken ct)
        {
            var document = await _dbContext
                .Documents
                .Include(x => x.DocumentTag)
                .SingleOrDefaultAsync(d => d.Id == id, ct);

            if (document != null)
            {
                foreach (var documentTag in document.DocumentTag)
                {
                    _dbContext.DocumentTags.Remove(documentTag);
                }

                document.IsDeleted = true;

                await _dbContext.SaveChangesAsync(ct);

                return document.Id;
            }

            return null;
        }

        private async Task ManageDocumentTags(Document document, IEnumerable<string> documentTags, CancellationToken ct)
        {
            var storedTags = await GetStoredTags(documentTags, ct);
            var newTags = GetNotStoredTags(documentTags, storedTags);

            document.DocumentTag = storedTags
                .Select(t => new DocumentTag { DocumentId = document.Id, TagId = t.Id })
                .ToList();

            foreach (var newTag in
                newTags.Select(x => new DocumentTag { Document = document, Tag = x }))
            {
                document.DocumentTag.Add(newTag);
            }
        }

        public async Task<Document> UpdateDocumentWithTagsAsync(Document documentToUpdate, IEnumerable<string> updatedDocumentTagNames, CancellationToken ct)
        {
            var document = await _dbContext
                .Documents
                .Include(x => x.DocumentTag)
                .SingleOrDefaultAsync(d => d.Id == documentToUpdate.Id && !d.IsDeleted, ct);
           
            if (document == null)
                throw new NotFoundException(nameof(Document), documentToUpdate.Id);

            document.RawJson = documentToUpdate.RawJson;

            foreach (var dt in document.DocumentTag)            
                _dbContext.DocumentTags.Remove(dt);            

            await ManageDocumentTags(document, updatedDocumentTagNames, ct);

            _dbContext.Documents.Update(document);
            await _dbContext.SaveChangesAsync();

            return document;
        }
    }
}
