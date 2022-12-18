using Notino.Application.Contracts.Messaging;
using Notino.Application.DTOs.Document;
using Notino.Application.Responses;

namespace Notino.Application.Features.Document.Requests.Commands.Common
{
    public abstract class BaseDocumentCommand : ICommand<Response>
    {
        public DocumentDto DocumentDto { get; set; }
    }
}
