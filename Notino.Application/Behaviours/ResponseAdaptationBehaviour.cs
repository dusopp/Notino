using MediatR;
using MessagePack;
using Newtonsoft.Json;
using Notino.Application.Constants.HttpHeaders;
using Notino.Application.Contracts.Messaging;
using Notino.Application.DTOs.Common;
using Notino.Application.Exceptions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notino.Application.Behaviours
{
    public sealed class ResponseAdaptationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class, IQueryRequest<TResponse>
        where TResponse: class, IRawResponseDto
    {

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {          
            
            var response = await next();            

            switch (request.AcceptHeader)
            {
                case AcceptHeaders.Json:
                    response.RawResponse = response.StoredValue;

                    break;
                case AcceptHeaders.Xml:
                    var xml = JsonConvert.DeserializeXNode(response.StoredValue, "root");
                    var xmlString = xml.ToString();
                    response.RawResponse = xmlString;
 
                    break;
                case AcceptHeaders.MessagePack:
                    var messagePack = MessagePackSerializer.ConvertFromJson(response.StoredValue);
                    var messagePackString = Encoding.UTF8.GetString(messagePack);
                    response.RawResponse = messagePackString;

                    break;
                default:
                    throw new NotSupportedMediaTypeException(request.AcceptHeader);
            }

            return response;
        }
    }
}
