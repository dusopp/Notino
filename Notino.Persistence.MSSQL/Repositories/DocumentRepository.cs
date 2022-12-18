using Microsoft.EntityFrameworkCore;
using Notino.Application.Contracts.Persistence;
using Notino.Application.Exceptions;
using Notino.Domain;
using Notino.Persistence.MSSQL.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notino.Persistence.MSSQL.Repositories
{
    public class DocumentRepository: BaseRepository<Document, string>, IDocumentRepository
    {
        private readonly DocumentDbContext _dbContext;

        public DocumentRepository(DocumentDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }       

        public async Task AddDocumentWithTagsAsync(Document document, IEnumerable<string> newDocumentTagNames)
        {  
            if (await ExistsAsync(document.Id))
                throw new AlreadyExistsException(nameof(Document), document.Id);

            var storedTags = await GetStoredTags(newDocumentTagNames);

            var newTags = GetNotStoredTags(newDocumentTagNames, storedTags);

            document.DocumentTag = storedTags
                .Select(t => new DocumentTag { DocumentId = document.Id, TagId = t.Id})
                .ToList();
           
            foreach (var newTag in 
                newTags.Select(x => new DocumentTag { Document = document, Tag = x }))
            {
                document.DocumentTag.Add(newTag);
            }
            
            await _dbContext.Documents.AddAsync(document);          
        }

        private async Task<List<Tag>> GetStoredTags(IEnumerable<string> tagNames)
        {
           return await _dbContext.Tags
                            .Where(x => tagNames.Contains(x.Name))
                            .Distinct()
                            .ToListAsync();
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

        public async Task DeleteDocumentWithTagsAsync(string id)
        {
            var document = await _dbContext
                .Documents
                .Include(x => x.DocumentTag)
                .SingleOrDefaultAsync(d => d.Id == id);

            if (document != null)
            {
                foreach (var documentTag in document.DocumentTag)
                {
                    _dbContext.DocumentTags.Remove(documentTag);
                }

                await DeleteAsync(document);
            }
            
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateDocumentWithTagsAsync(Document documentToUpdate, IEnumerable<string> updatedDocumentTagNames)
        {
            var document = await _dbContext
                .Documents
                .Include(x => x.DocumentTag)                
                .SingleOrDefaultAsync(d => d.Id == documentToUpdate.Id);

            if (document == null)
                throw new NotFoundException(nameof(Document), document.Id);

            document.RawJson = documentToUpdate.RawJson;

            var storedTags = await GetStoredTags(updatedDocumentTagNames);
            var newTags = GetNotStoredTags(updatedDocumentTagNames, storedTags);

            document.DocumentTag = storedTags
                    .Select(t => new DocumentTag { DocumentId = document.Id, TagId = t.Id })                    
                    .ToList();

            foreach (var newTag in
                newTags.Select(x => new DocumentTag { Document = document, Tag = x }))
            {
                document.DocumentTag.Add(newTag);
            }

            var storedTagsToRemove = storedTags.Where(t => !updatedDocumentTagNames.Contains(t.Name));

            foreach (var tag in storedTagsToRemove)
            {
                _dbContext.DocumentTags.Remove(
                    new DocumentTag { DocumentId = documentToUpdate.Id, TagId = tag.Id });
            }
        }
    }
}
