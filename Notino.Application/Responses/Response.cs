using Notino.Application.Responses.Common;
using System.Collections.Generic;

namespace Notino.Application.Responses
{
    public class Response : BaseResponse
    {
        //tototo
        public static Response CreateErrorResponse(
                int statusCode,
                string message,
                IEnumerable<string> errors
            ) => new Response
            {
                StatusCode = statusCode,
                Success = false,
                Message = message,
                Errors = errors
            };
    }
}
