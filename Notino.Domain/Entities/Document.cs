using Notino.Domain.Entities.Common;
using System.Collections.Generic;

namespace Notino.Domain.Entities
{

    public class Document : BaseDomainEntityWithRawJson<string>
    {
        public ICollection<DocumentTag> DocumentTag { get; set; }
    }
}
