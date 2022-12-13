﻿using MediatR;
using Newtonsoft.Json;
using Notino.Application.Constants.Validation;
using Notino.Application.Contracts.Caching;
using Notino.Application.Contracts.Persistence;
using Notino.Application.DTOs.Document;
using Notino.Application.Exceptions;
using Notino.Application.Features.Document.Requests.Queries;
using System.Threading;
using System.Threading.Tasks;

namespace Notino.Application.Features.Document.Handlers.Queries
{
    public class GetDocumentHandler : IRequestHandler<GetDocumentRequest, DocumentDto>
    {
        private readonly IApplicationState _applicationState;
        private readonly IUnitOfWork _unitOfWork;        

        public GetDocumentHandler(
            IApplicationState applicationState,
            IUnitOfWork unitOfWork
            )
        {
            _applicationState = applicationState;
            _unitOfWork = unitOfWork;            
        }

        public async Task<DocumentDto> Handle(GetDocumentRequest request, CancellationToken cancellationToken)
        {
          
            var document = await _unitOfWork
                .DocumentRepository
                .GetById(request.Id);

            if (document == null)
                throw new NotFoundException(ValidationMessages.Id, request.Id);

            var documentDto = JsonConvert.DeserializeObject<DocumentDto>(document.Value);

            documentDto.StoredValue = document.Value;

            return documentDto;
        }
    }
}
