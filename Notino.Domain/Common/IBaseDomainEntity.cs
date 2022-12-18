using System;
using System.Collections.Generic;
using System.Text;

namespace Notino.Domain.Common
{
    public interface IBaseDomainEntity<TKey>
    {
        TKey Id { get; set; }

        
    }
}
