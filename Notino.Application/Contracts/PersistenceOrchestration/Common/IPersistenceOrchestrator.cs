using Notino.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notino.Application.Contracts.PersistenceOrchestration
{
    public interface IPersistenceOrchestrator<TKey>
    {       

        Task RevertAsync(List<Task<TKey>> failedTasks, 
            TKey id, 
            Dictionary<int, 
            Func<TKey, Task<TKey>>> revertMethods,
            int revertsCnt = 0);

    }
}
