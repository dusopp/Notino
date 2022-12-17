namespace Notino.Application.DTOs.Common
{
    public interface IBaseDto<TKey>
    {
        public TKey Id { get; set; }
    }
}
