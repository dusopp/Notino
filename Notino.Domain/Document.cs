using Notino.Domain.Common;
using System.Collections.Generic;

namespace Notino.Domain
{

    public class Document : BaseDomainEntity<string>
    {      
        public ICollection<DocumentTag> DocumentTag { get; set; }

        public string Value { get; set; }
    }
}
