using Newtonsoft.Json;
using Notino.Application.Contracts.ResponseConversion.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
