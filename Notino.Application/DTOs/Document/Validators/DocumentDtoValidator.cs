using FluentValidation;
using Notino.Application.Constants.Validation;
using Notino.Application.Features.Document.Requests.Commands.Common;

namespace Notino.Application.DTOs.Document.Validators
{
    public class DocumentDtoValidator : AbstractValidator<IBaseDocumentCommand>
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
