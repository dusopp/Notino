namespace Notino.Domain.Common
{
    public interface IBaseDomainEntity<TKey>: ISoftDeletableEntity
    {
        TKey Id { get; set; }

    }
}
