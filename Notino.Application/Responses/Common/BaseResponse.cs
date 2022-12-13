using System;
using System.Collections.Generic;
using System.Text;

namespace Notino.Application.Responses.Common
{
    public class BaseResponse
    {
        public int StatusCode { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }

        //public string Body { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}
