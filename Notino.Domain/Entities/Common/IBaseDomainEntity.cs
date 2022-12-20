namespace Notino.Domain.Entities.Common
{
    public interface IBaseDomainEntity<TKey>: ISoftDeletableEntity
    {
        TKey Id { get; set; }

    }
}
