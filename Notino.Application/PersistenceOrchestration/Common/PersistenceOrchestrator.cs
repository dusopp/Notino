using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Notino.Application.PersistenceOrchestration.Common
{
    /*
     * Zvazenie     
     chcem docielit, aby orchestratory obsahovali revert metodu,
     aby som donutil developerov implementova tuto metodu,
     aby si uvedomili ze transakcie je nutne po chybe revertovat
     */
    public abstract class PersistenceOrchestrator<TKey> 
    {
        protected abstract Task Revert(List<Task<TKey>> tasks, TKey id);
    }
}
