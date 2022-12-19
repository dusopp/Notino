using System.Threading.Tasks;

namespace Notino.Application.Contracts.ResponseConversion.Converters
{
    public interface IResponseConverter
    {
        string Convert(string json);
    }
}
