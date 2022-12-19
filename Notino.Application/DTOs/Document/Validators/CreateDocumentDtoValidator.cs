using FluentValidation;
using Notino.Application.Features.Document.Requests.Commands;

namespace Notino.Application.DTOs.Document.Validators
{
    public class CreateDocumentDtoValidator : AbstractValidator<CreateDocumentCommand>
    {
        public CreateDocumentDtoValidator()
        {
            Include(new DocumentDtoValidator());
        }
    }

}
