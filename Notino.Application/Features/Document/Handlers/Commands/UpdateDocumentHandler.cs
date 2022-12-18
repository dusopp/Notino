using MediatR;
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
    public class UpdateDocumentHandler : ICommandHandler<UpdateDocumentCommand, Response>
    {
        private readonly IDocumentPersistenceOrchestrator _storageOrchestrator;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDocumentHandler(
            IDocumentPersistenceOrchestrator storageOrchestrator,
            IUnitOfWork unitOfWork
            )
        {
            _storageOrchestrator = storageOrchestrator;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<Response> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
        {         
            var newDocument = new Domain.Document()
            {
                Id = request.DocumentDto.Id,
                RawJson = JsonConvert.SerializeObject(request.DocumentDto)
            };

            await _storageOrchestrator.UpdateAsync(newDocument, request.DocumentDto.Tags);

            return new Response();
        }
    }
}
