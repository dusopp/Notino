using System;

namespace Notino.Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string entityType, string entityKeyValue) 
            : base($"{entityType} with Id:'{entityKeyValue}' was not found")
        {

        }
    }
}
