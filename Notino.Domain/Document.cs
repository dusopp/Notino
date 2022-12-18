using Notino.Domain.Common;
using System.Collections.Generic;

namespace Notino.Domain
{

    public class Document : BaseDomainEntityWithRawJson<string>
    {      
        public ICollection<DocumentTag> DocumentTag { get; set; }
    }
}
