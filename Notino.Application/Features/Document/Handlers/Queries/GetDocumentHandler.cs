using MediatR;
using Notino.Application.Constants.Validation;
using Notino.Application.Contracts.Persistence;
using Notino.Application.DTOs.Common;
using Notino.Application.Exceptions;
using Notino.Application.Features.Document.Requests.Queries;
using System.Threading;
using System.Threading.Tasks;

namespace Notino.Application.Features.Document.Handlers.Queries
{
    public class GetDocumentHandler : IRequestHandler<GetDocumentRequest, RawResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;        

        public GetDocumentHandler(IUnitOfWork unitOfWork)
        {            
            _unitOfWork = unitOfWork;            
        }

        public async Task<RawResponseDto> Handle(GetDocumentRequest request, CancellationToken ct)
        {
          
            var document = await _unitOfWork
                .DocumentRepository
                .GetByIdAsync(request.Id, ct);

            if (document == null)
                throw new NotFoundException(ValidationMessages.Id, request.Id);

            var rawReposnseDto = new RawResponseDto()
            { 
                StoredValue = document.RawJson
            };

            return rawReposnseDto;
        }
    }
}
