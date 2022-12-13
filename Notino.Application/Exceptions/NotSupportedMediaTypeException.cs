using System;
using System.Collections.Generic;
using System.Text;

namespace Notino.Application.Exceptions
{
    public class NotSupportedMediaTypeException : ApplicationException
    {
        public NotSupportedMediaTypeException(string mediaType)
            : base($"Mediatype: '{mediaType}' is not supported")
        {

        }
    }
}
