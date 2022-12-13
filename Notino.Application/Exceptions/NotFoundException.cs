using System;

namespace Notino.Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string keyName, string keyValue) 
            : base($"{keyName}:'{keyValue}' was not found")
        {

        }
    }
}
