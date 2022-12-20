using System.Threading.Tasks;

namespace Notino.Domain.Contracts.ResponseConversion.Converters
{
    public interface IResponseConverter
    {
        string Convert(string json);
    }
}
