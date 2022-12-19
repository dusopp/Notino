using MediatR;
using MessagePack;
using Newtonsoft.Json;
using Notino.Application.Constants.HttpHeaders;
using Notino.Application.Contracts.Messaging;
using Notino.Application.Contracts.ResponseConversion.Factory;
using Notino.Application.DTOs.Common;
using Notino.Application.Exceptions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notino.Application.Behaviours
{
    public sealed class ResponseConversionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class, IQueryRequest<TResponse>
        where TResponse: class, IRawResponseDto
    {
        private readonly IResponseConverterFactory _converterFactory;

        public ResponseConversionBehaviour(IResponseConverterFactory converterFactory)
        {
            _converterFactory = converterFactory;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {         
            var response = await next();

            var responseConverter = _converterFactory.Create(request.AcceptHeader);
            response.RawResponse = responseConverter.Convert(response.RawResponse);

            return response;
        }
    }
}
