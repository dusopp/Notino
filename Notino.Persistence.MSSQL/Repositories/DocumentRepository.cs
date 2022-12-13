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

        public async Task<string> AddDocumentWithTagsAsync(Document document, IEnumerable<string> tagNames)
        {


            var documentExists = await _dbContext
                .Documents
                .SingleOrDefaultAsync(x => x.Id == document.Id);

            if (documentExists != null)
                throw new BadRequestException($"Document with Id: {document.Id} already exists");

            var foundTags = await _dbContext.Tags               
                .Where(x => tagNames.Contains(x.Name))
                .Select(x => x.Name)
                .Distinct()
                .ToListAsync();

            var notFoundTags = tagNames
                .Where(c => !foundTags.Any(x => x == c.ToLower()))
                .ToList();


            if (notFoundTags.Count != 0)
            {
                await _dbContext
                    .Tags
                    .AddRangeAsync(
                        notFoundTags.Select(tagName => new Tag { Name = tagName})
                    );
            }

            var tagsToMap = await _dbContext.Tags
                        .Where(x => tagNames.Contains(x.Name))
                        .Distinct()
                        .ToListAsync();

            document.DocumentTag = tagsToMap
                .Select(t => new DocumentTag { DocumentId = document.Id, TagId = t.Id})
                .ToList();

            await _dbContext.Documents.AddAsync(document);            

            return document.Id;
        }

        public Task DeleteDocumentAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Document> GetDocumentAsync(string documentId)
        {
            return await _dbContext
                .Documents
                .SingleOrDefaultAsync(x => x.Id == documentId);
        }
    }
}
