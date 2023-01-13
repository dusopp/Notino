using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notino.API.Controllers.Common;
using Notino.Application.DTOs.Document;
using Notino.Application.Features.Document.Requests.Commands;
using Notino.Application.Features.Document.Requests.Queries;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notino.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentController : BaseController
    {
        

        public DocumentController(IMediator mediator)
            :base(mediator)
        {            
        }
        
        [HttpGet("{documentId}")]
        [ProducesResponseType(typeof(DocumentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public async Task<IActionResult> GetDocument(string documentId, CancellationToken ct)
        {
            Request.Headers.TryGetValue("Accept", out var acceptHeader);

            var document = await _mediator
                .Send(
                    new GetDocumentRequest
                    {
                        Id = documentId,
                        AcceptHeader = acceptHeader,
                        BypassCache = false
                    },
                    ct
                ); 

            return Content(document.RawResponse, acceptHeader, Encoding.UTF8);            
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateDocument([FromBody] DocumentDto document, CancellationToken ct)
        {
            var command = new CreateDocumentCommand { DocumentDto = document };
            await _mediator.Send(command, ct);
            return Created(Request.Path, null);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDocument([FromBody] DocumentDto document, CancellationToken ct)
        {
            var command = new UpdateDocumentCommand { DocumentDto = document };
            await _mediator.Send(command, ct);
            return NoContent();
        }
    }
}
