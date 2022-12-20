using Notino.Domain.Contracts.ResponseConversion.Converters;

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
