using System;
using System.Collections.Generic;
using System.Text;

namespace Notino.Domain.Common
{
    public interface IBaseDomainEntityWithRawJson<TKey> : 
        IBaseDomainEntity<TKey>, 
        IEntityWithRawJson
    {
    }
}
