using Notino.Application.Contracts.ResponseConversion.Converters;
using System.Threading.Tasks;

namespace Notino.Application.ResponseConversion.Converters
{
    public class JsonConverter : IResponseConverter
    {
        public string Convert(string json)
        {
            return json;
        }
    }
}
