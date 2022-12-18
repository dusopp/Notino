using Notino.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notino.Application.Contracts.PersistenceOrchestration
{
    public interface IDocumentPersistenceOrchestrator
    {
        Task AddAsync(Document entity, IEnumerable<string> tagNames);

        Task UpdateAsync(Document entity, IEnumerable<string> tagNames);

        Task RevertAsync(List<Task> failedTasks,
            string id,
            Dictionary<int,
            Func<string, Task>> revertMethods,
            int revertsCnt = 0);
    }
}
