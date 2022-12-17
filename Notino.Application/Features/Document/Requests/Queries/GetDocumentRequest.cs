using MediatR;
using Notino.Application.Contracts.Messaging;
using Notino.Application.DTOs.Common;
using Notino.Application.DTOs.Document;
using System;

namespace Notino.Application.Features.Document.Requests.Queries
{
    public class GetDocumentRequest : ICacheableQuery<RawResponseDto>
    {
        public string Id { get; set; }        

        public string AcceptHeader { get; set; }

        public bool BypassCache { get; set; }

        public string CacheKey => $"{nameof(Document)}-{Id}"; //-{AcceptHeader}

        public TimeSpan? SlidingExpiration { get; set; }
    }
}
