using System;
using System.Collections.Generic;
using System.Text;

namespace Notino.Domain.Common
{
    /// <summary>
    /// Basic interface for an entity
    /// </summary>
    /// <typeparam name="TKey">Type of the entity identity property.</typeparam>
    public interface IBaseDomainEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
