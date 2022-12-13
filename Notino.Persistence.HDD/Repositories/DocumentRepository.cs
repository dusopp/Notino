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
        private const string FileNameFormat = "document{0}.txt";

        public async Task<string> AddDocumentWithTagsAsync(Document document, IEnumerable<string> tagNames)
        {
            await File.AppendAllTextAsync(GetFileName(document.Id), document.Value);
            return document.Id;
        }


        public async Task DeleteDocumentAsync(string id)
        {
            //toto should add try catch
            Task t = Task.Run(() => File.Delete(GetFileName(id)));

            await t;           
        }

    }
}
