namespace Notino.Domain.Common
{
    public interface IEntityWithRawJson
    {
        string RawJson { get; set; }

        int InternalId { get; set; }
    }
}
