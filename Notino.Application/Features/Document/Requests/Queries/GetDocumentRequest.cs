using MediatR;
using Notino.Application.Contracts.Messaging;
using Notino.Application.DTOs.Document;
using System;

namespace Notino.Application.Features.Document.Requests.Queries
{
    public class GetDocumentRequest : ICacheableQuery<DocumentDto>
    {
        public string Id { get; set; }        

        public string AcceptHeader { get; set; }

        public bool BypassCache { get; set; }

        public string CacheKey => $"{nameof(Document)}-{Id}";

        public TimeSpan? SlidingExpiration { get; set; }
    }
}
