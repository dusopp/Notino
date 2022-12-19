using System;

namespace Notino.Domain.Common
{
    public abstract class BaseDomainEntityWithRawJson<TKey> 
        : IBaseDomainEntityWithRawJson<TKey>, IAuditableEntity, ISoftDeletableEntity
    {
        public TKey Id { get; set; }

        public string RawJson { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public int InternalId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
