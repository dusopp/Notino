using Notino.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notino.Application.Contracts.PersistenceOrchestration
{
    public interface IDocumentPersistenceOrchestrator
    {
        public Task AddAsync(Document document, IEnumerable<string> tagNames);


    }
}
