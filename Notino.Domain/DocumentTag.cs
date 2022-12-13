using System;
using System.Collections.Generic;
using System.Text;

namespace Notino.Domain
{
    public class DocumentTag
    {
        public string DocumentId { get; set; }

        public Document Document { get; set; }

        public int TagId { get; set; }

        public Tag Tag { get; set; }
    }
}
