using FluentValidation;
using Notino.Application.Features.Document.Requests.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notino.Application.DTOs.Document.Validators
{
    public class UpdateDocumentDtoValidator : AbstractValidator<UpdateDocumentCommand>
    {
        public UpdateDocumentDtoValidator()
        {
            Include(new DocumentDtoValidator());
        }
    }
}
