using System;
using System.Collections.Generic;
using System.Text;

namespace Notino.Application.DTOs.Common
{
    public class RawResponseDto : IRawResponseDto
    {
        public string RawResponse { get; set; }
        public string StoredValue { get; set; }
    }
}
