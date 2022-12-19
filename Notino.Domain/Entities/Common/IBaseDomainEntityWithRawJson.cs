using System;
using System.Collections.Generic;
using System.Text;

namespace Notino.Domain.Entities.Common
{
    public interface IBaseDomainEntityWithRawJson<TKey> : 
        IBaseDomainEntity<TKey>, 
        IEntityWithRawJson
    {
    }
}
