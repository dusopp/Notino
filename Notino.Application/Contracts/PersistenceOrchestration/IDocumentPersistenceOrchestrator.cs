using Notino.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Notino.Application.Contracts.PersistenceOrchestration
{
    public interface IDocumentPersistenceOrchestrator
    {
        Task AddAsync(Document entity, IEnumerable<string> tagNames, CancellationToken ct);

        Task UpdateAsync(Document entity, IEnumerable<string> tagNames, CancellationToken ct);
    }
}
