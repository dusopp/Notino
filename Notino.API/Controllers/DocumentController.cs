using Firebase.Database;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Notino.API.Controllers.Common;
using Notino.Application.DTOs.Document;
using Notino.Application.Features.Document.Requests.Commands;
using Notino.Application.Features.Document.Requests.Queries;
using Notino.Application.Responses;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Notino.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentController : BaseController
    {
        private readonly IMediator _mediator;

        public DocumentController(IMediator mediator)
            :base()
        {
            _mediator = mediator;
        }
        
        [HttpGet("{documentId}")]
        [ProducesResponseType(typeof(DocumentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public async Task<IActionResult> GetDocument(string documentId)
        {
            Request.Headers.TryGetValue("Accept", out var acceptHeader);

            var document = await _mediator
                .Send(
                    new GetDocumentRequest
                    {
                        Id = documentId,
                        AcceptHeader = acceptHeader,
                        BypassCache = false
                    }
                ); 

            return Content(document.RawResponse, acceptHeader, Encoding.UTF8);            
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateDocument([FromBody] DocumentDto document)
        {
            var command = new CreateDocumentCommand { DocumentDto = document };
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDocument([FromBody] DocumentDto document)
        {
            var command = new UpdateDocumentCommand { DocumentDto = document };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
