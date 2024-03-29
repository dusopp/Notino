﻿namespace Notino.Domain.Entities
{
    public class DocumentTag
    {
        public string DocumentId { get; set; }

        public int DocumentInternalId { get; set; }

        public Document Document { get; set; }

        public int TagId { get; set; }

        public Tag Tag { get; set; }
    }
}
