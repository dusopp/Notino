using Notino.Application.Contracts.PersistenceOrchestration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notino.Application.PersistenceOrchestration.Common
{
    /*   
    
    For future consideration, think if generic solution is not overkill.   
     
     */
    public abstract class BasePersistenceOrchestrator<TKey> : IDocumentPersistenceOrchestrator        
    {
        public abstract Task AddAsync(Domain.Document entity, IEnumerable<string> tagNames);

        public async Task RevertAsync(
            List<Task> failedTasks,
            string id,
            Dictionary<int, Func<string, Task>> revertFuncs,
            int revertCnt = 0
            )
        {   
            var revertMethodsDict = new Dictionary<int, Func<string, Task>>();
            var revertTasks = new List<Task>();

            for (int i = 0; i < failedTasks.Count; i++)
            {
                if (!failedTasks[i].IsFaulted)
                {
                    revertMethodsDict.Add(i, revertFuncs[i]);
                    revertTasks.Add(revertFuncs[i](id));
                }
                else
                {
                    revertFuncs.Remove(i);
                }
            }

            var result = Task.WhenAll(revertTasks);
            try
            {
                await result;
            }
            catch
            {
                if (revertCnt > 3)
                    throw;

                await RevertAsync(revertTasks, id, revertFuncs, ++revertCnt);
            }
        }
    }
}
