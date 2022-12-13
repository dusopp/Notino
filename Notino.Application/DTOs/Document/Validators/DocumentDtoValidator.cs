using FluentValidation;
using Notino.Application.Constants.Validation;
using Notino.Application.Features.Document.Requests.Commands;

namespace Notino.Application.DTOs.Document.Validators
{
    public class DocumentDtoValidator : AbstractValidator<CreateDocumentCommand>
    {
        public DocumentDtoValidator()
        {
            RuleFor(p => p.DocumentDto.Id)                
                .NotEmpty().WithMessage(ValidationMessages.PropertyRequired);

            RuleFor(p => p.DocumentDto.Tags)
                .NotNull().WithMessage(ValidationMessages.PropertyRequired);

            RuleFor(p => p.DocumentDto.Data)
                .NotNull().WithMessage(ValidationMessages.PropertyRequired);

        }
    }
}
