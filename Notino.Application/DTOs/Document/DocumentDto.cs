using Newtonsoft.Json;
using Notino.Application.DTOs.Common;
using System.Collections.Generic;
using System.Dynamic;

namespace Notino.Application.DTOs.Document
{
    public class DocumentDto : BaseDto<string>, IRawResponseDto
    {
        public List<string> Tags { get; set; }

        public ExpandoObject Data { get; set; }

        
        public string RawResponse { get; set; }

        
        public string StoredValue { get; set; }

    }
}
