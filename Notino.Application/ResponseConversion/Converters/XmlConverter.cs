using Newtonsoft.Json;
using Notino.Domain.Contracts.ResponseConversion.Converters;

namespace Notino.Application.ResponseConversion.Converters
{
    public class XmlConverter : IResponseConverter
    {
        public string Convert(string json)
        {
            var xml = JsonConvert.DeserializeXNode(json, "root");
            return xml.ToString();
        }
    }
}
