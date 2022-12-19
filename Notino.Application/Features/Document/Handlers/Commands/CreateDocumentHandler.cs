﻿using Newtonsoft.Json;
using Notino.Application.Contracts.Messaging;
using Notino.Application.Contracts.Persistence;
using Notino.Application.Contracts.PersistenceOrchestration;
using Notino.Application.Features.Document.Requests.Commands;
using Notino.Application.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Notino.Application.Features.Document.Handlers.Commands
{
    public class CreateDocumentHandler : ICommandHandler<CreateDocumentCommand, Response>
    {
        private readonly IDocumentPersistenceOrchestrator _docStorageOrchestrator;
        private readonly IUnitOfWork unitOfWork;        

        public CreateDocumentHandler(
            IDocumentPersistenceOrchestrator docStorageOrchestrator 
            //, IUnitOfWork unitOfWork //to delete for testing   
            )
        {
            _docStorageOrchestrator = docStorageOrchestrator ??
                throw new ArgumentNullException(nameof(docStorageOrchestrator));

            //this.unitOfWork = unitOfWork;            
        }
        
        //refactor return type
        public async Task<Response> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
        {         
            var newDocument = new Domain.Document()
            {
                Id = request.DocumentDto.Id,
                RawJson = JsonConvert.SerializeObject(request.DocumentDto)
            };

            await _docStorageOrchestrator.AddAsync(newDocument, request.DocumentDto.Tags);

            //await unitOfWork.DocumentRepository.DeleteDocumentWithTagsAsync(newDocument.Id);

            //await unitOfWork.DocumentRepository.UpdateDocumentWithTagsAsync(newDocument, request.DocumentDto.Tags);
            //await unitOfWork.SaveAsync();

            return new Response();
        }
    }
}
