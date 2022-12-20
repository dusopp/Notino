using Notino.Domain.Entities.Common;
using System.Collections.Generic;

namespace Notino.Domain.Entities
{
    public class Tag : BaseDomainEntity<int>
    {
        public ICollection<DocumentTag> DocumentTag { get; set; }

        public string Name { get; set; }
    }
}
