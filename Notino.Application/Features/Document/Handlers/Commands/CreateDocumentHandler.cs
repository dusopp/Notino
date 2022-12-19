using Newtonsoft.Json;
using Notino.Application.Contracts.Messaging;
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
             

        public CreateDocumentHandler(
            IDocumentPersistenceOrchestrator docStorageOrchestrator
        )
        {
            _docStorageOrchestrator = docStorageOrchestrator ??
                throw new ArgumentNullException(nameof(docStorageOrchestrator));                  
        }        
        
        public async Task<Response> Handle(CreateDocumentCommand request, CancellationToken ct)
        {         
            var newDocument = new Domain.Entities.Document()
            {
                Id = request.DocumentDto.Id,
                RawJson = JsonConvert.SerializeObject(request.DocumentDto)
            };

            await _docStorageOrchestrator.AddAsync(newDocument, request.DocumentDto.Tags, ct);

            //await unitOfWork.DocumentRepository.DeleteDocumentWithTagsAsync(newDocument.Id);

            //await unitOfWork.DocumentRepository.UpdateDocumentWithTagsAsync(newDocument, request.DocumentDto.Tags);
            //await unitOfWork.SaveAsync();

            return new Response() { Success = true };
        }
    }
}
