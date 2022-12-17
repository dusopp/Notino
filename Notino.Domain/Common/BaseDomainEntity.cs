namespace Notino.Domain.Common
{
    public abstract class BaseDomainEntity<TKey> :  IBaseDomainEntity<TKey>
    {
        public TKey Id { get; set; }
        public string RawJson { get; set; }
    }
}
