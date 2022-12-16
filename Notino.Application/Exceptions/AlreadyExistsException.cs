using System;
using System.Collections.Generic;
using System.Text;

namespace Notino.Application.Exceptions
{
    public class AlreadyExistsException : ApplicationException
    {
        public AlreadyExistsException(string entityType, string entityKeyValue)
            : base($"{entityType} with Id:'{entityKeyValue}' already exists")
        {

        }
    }
}
