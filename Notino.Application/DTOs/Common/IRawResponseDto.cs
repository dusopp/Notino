using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;


namespace Notino.Application.DTOs.Common
{
    public interface IRawResponseDto
    {
       
        public string RawResponse { get; set; }

        
        public string StoredValue { get; set; }
    }
}
