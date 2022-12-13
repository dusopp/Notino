using System;
using System.Collections.Generic;
using System.Text;

namespace Notino.Application.Contracts.Caching
{
    public interface IApplicationState
    {
        TItem Get<TItem>(string key);
        bool Has<TItem>(string key);
        void Set<TItem>(string key, TItem value) where TItem : notnull;
    }
}
