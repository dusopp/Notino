using System;

namespace Notino.Domain.Common
{
    public interface IAuditableEntity
    {
        public DateTime DateCreated { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
