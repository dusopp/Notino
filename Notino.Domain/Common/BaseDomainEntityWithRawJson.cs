namespace Notino.Domain.Common
{
    public abstract class BaseDomainEntityWithRawJson<TKey> : IBaseDomainEntityWithRawJson<TKey>
    {
        public TKey Id { get; set; }

        public string RawJson { get; set; }
    }
}
