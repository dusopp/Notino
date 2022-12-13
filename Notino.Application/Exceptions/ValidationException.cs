using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notino.Application.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public List<string> Errors { get; set; }

        public ValidationException(IEnumerable<ValidationFailure> errors)
        {
            Errors = new List<string>();

            foreach (var error in errors)
            {
                Errors.Add(error.ErrorMessage);
            }
        }
    }
}
