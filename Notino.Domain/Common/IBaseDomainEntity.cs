namespace Notino.Domain.Common
{
    public interface IBaseDomainEntity<TKey>
    {
        TKey Id { get; set; }

    }
}
