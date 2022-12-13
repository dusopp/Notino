namespace Notino.Application.DTOs.Common
{
    public abstract class BaseDto<TKey>
    {
        public TKey Id { get; set; }
    }
}
