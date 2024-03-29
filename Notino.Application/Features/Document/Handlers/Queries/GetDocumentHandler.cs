﻿using MediatR;
using Notino.Application.DTOs.Common;
using Notino.Application.Exceptions;
using Notino.Application.Features.Document.Requests.Queries;
using Notino.Domain.Contracts.Persistence;
using System.Threading;
using System.Threading.Tasks;

namespace Notino.Application.Features.Document.Handlers.Queries
{
    public class GetDocumentHandler : IRequestHandler<GetDocumentRequest, RawResponseDto>
    {
        private readonly IDocumentRepository _docRepo;

        public GetDocumentHandler(IDocumentRepository docRepo)
        {          
            _docRepo = docRepo;
        }

        public async Task<RawResponseDto> Handle(GetDocumentRequest request, CancellationToken ct)
        {          
            var document = await _docRepo                
                .GetByIdAsync(request.Id, ct);

            if (document == null)
                throw new NotFoundException(nameof(Domain.Entities.Document), request.Id);

            return new RawResponseDto()
            { 
                RawResponse = document.RawJson
            };            
        }
    }
}
