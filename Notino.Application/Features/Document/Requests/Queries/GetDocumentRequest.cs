using MediatR;
using Notino.Application.Contracts.Messaging;
using Notino.Application.DTOs.Document;

namespace Notino.Application.Features.Document.Requests.Queries
{
    public class GetDocumentRequest : IQueryRequest<DocumentDto>
    {
        public string Id { get; set; }        

        public string AcceptHeader { get; set; }
    }
}
