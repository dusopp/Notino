using Newtonsoft.Json;
using Notino.Application.Contracts.Messaging;
using Notino.Application.Contracts.Persistence;
using Notino.Application.Contracts.PersistenceOrchestration;
using Notino.Application.Features.Document.Requests.Commands;
using Notino.Application.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Notino.Application.Features.Document.Handlers.Commands
{
    public class CreateDocumentHandler : ICommandHandler<CreateDocumentCommand, Response>
    {
        private readonly IDocumentPersistenceOrchestrator storageOrchestrator;
        private readonly IDocumentRepository _documentRepository;

        public CreateDocumentHandler(IDocumentPersistenceOrchestrator storageOrchestrator, IDocumentRepository documentRepository)
        {
            this.storageOrchestrator = storageOrchestrator;
            _documentRepository = documentRepository;
        }

        //tototo return type skontrolovat
        public async Task<Response> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
        {
            

            var response = new Response();

            var newDocument = new Domain.Document()
            {
                Id = request.DocumentDto.Id,
                Value = JsonConvert.SerializeObject(request.DocumentDto)
            };

            await storageOrchestrator.AddAsync(newDocument, request.DocumentDto.Tags);

            //await _documentRepository.AddDocumentWithTagsAsync(newDocument, request.DocumentDto.Tags);
 
            return response;
        }
    }
}
