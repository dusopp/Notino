﻿using Notino.Domain.Common;
using System.Collections.Generic;

namespace Notino.Domain
{
    public class @string : BaseDomainEntity<int>
    {
        public ICollection<DocumentTag> DocumentTag { get; set; }

        public string Name { get; set; }        
    }
}
