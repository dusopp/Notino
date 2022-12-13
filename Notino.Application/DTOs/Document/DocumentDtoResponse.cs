using Notino.Application.DTOs.Common;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Notino.Application.DTOs.Document
{
    public class DocumentDtoResponse : BaseDto<string>
    {
        public List<string> Tags { get; set; }

        public dynamic Data { get; set; }


    }
}
