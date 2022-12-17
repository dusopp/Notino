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

        public async Task AddDocumentWithTagsAsync(Document document, IEnumerable<string> tagNames)
        {            
            //tototo
            if (await ExistsAsync(document.Id))
                throw new AlreadyExistsException(nameof(Document), document.Id); 

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
                        notFoundTags.Select(tagName => new @string { Name = tagName})
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
            //remove
            await _dbContext.SaveChangesAsync();
        }

    }
}
