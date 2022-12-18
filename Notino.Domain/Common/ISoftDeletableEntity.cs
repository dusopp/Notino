namespace Notino.Domain.Common
{
    public interface ISoftDeletableEntity
    {
        bool IsDeleted { get; set; }
    }
}
