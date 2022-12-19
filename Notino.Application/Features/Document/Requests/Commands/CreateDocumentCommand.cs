using Notino.Application.DTOs.Document;
using Notino.Application.Features.Document.Requests.Commands.Common;

namespace Notino.Application.Features.Document.Requests.Commands
{
    public class CreateDocumentCommand : IBaseDocumentCommand
    {
        public DocumentDto DocumentDto { get; set; }
    }
}
