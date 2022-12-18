using Notino.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notino.Application.Contracts.PersistenceOrchestration
{
    public interface IDocumentPersistenceOrchestrator : IPersistenceOrchestrator<string>
    {
        Task AddAsync(Document entity, IEnumerable<string> tagNames);

        //Task UpdateAsync(Document entity, IEnumerable<string> tagNames);
    }
}
