using System;
          
namespace Notino.Domain.Entities.Common
{
    public abstract class BaseDomainEntity<TKey> 
        : IBaseDomainEntity<TKey>, IAuditableEntity, ISoftDeletableEntity
    {
        public TKey Id { get; set; }      

        public DateTime DateCreated { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
