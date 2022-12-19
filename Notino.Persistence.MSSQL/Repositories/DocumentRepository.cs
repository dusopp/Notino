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
        private readonly NotinoDbContext _dbContext;

        public DocumentRepository(NotinoDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }       

        public async Task<Document> AddDocumentWithTagsAsync
            (Document document, 
            IEnumerable<string> newDocumentTagNames,
            bool isUpdate = false)
        {
            var storedDocument = await GetByIdAsync(document.Id);

            if (storedDocument != null && !isUpdate)
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
            
            return document;
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

        public async Task<string> DeleteDocumentWithTagsAsync(string id)
        {
            var document = await _dbContext
                .Documents
                .Include(x => x.DocumentTag)
                .SingleOrDefaultAsync(d => d.Id == id);

            if(document == null)
                throw new NotFoundException(nameof(Document), id);
          
            foreach (var documentTag in document.DocumentTag)
            {
                _dbContext.DocumentTags.Remove(documentTag);
            }

            document.IsDeleted = true;              
           
            return document.Id;
        }

        public async Task<Document> UpdateDocumentWithTagsAsync(Document documentToUpdate, IEnumerable<string> updatedDocumentTagNames)
        {
            var document = await _dbContext
                .Documents
                .Include(x => x.DocumentTag)                
                .SingleOrDefaultAsync(d => d.Id == documentToUpdate.Id && !d.IsDeleted);

            if (document == null)
                throw new NotFoundException(nameof(Document), documentToUpdate.Id);

            document.IsDeleted = true;           
            await AddDocumentWithTagsAsync(documentToUpdate, updatedDocumentTagNames, true);

            return documentToUpdate;

            /*
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
            */
        }
    }
}
