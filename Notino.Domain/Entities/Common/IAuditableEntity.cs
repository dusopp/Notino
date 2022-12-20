using System;

namespace Notino.Domain.Entities.Common
{
    public interface IAuditableEntity
    {
        public DateTime DateCreated { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
