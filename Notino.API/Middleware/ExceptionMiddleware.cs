using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Notino.Application.Exceptions;
using Notino.Application.Responses;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Notino.API.Middleware
{
    /*
     Toto by malo ist po spravnosti do Application vrstvy,
     len uz nie je cas to menit.
     Implementacia v Application vrstve by bola cez IPipelineBehavior (viac menej len copy paste
     nizsie uvedeneho kodu),
     zabezpeci sa tym rovnka funkcionalita. Solution by bolo tym padom decoupled
     od asp.net core
     */
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

               
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (OperationCanceledException opex)
            {
                _logger.LogInformation($"Request cancelled: {httpContext.Request.Path}");
                await HandleExceptionAsync(httpContext, opex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error: {httpContext.Request.Path}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {            
            httpContext.Response.ContentType = "application/json";
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string errorMessage = "Internal server error.";
            IEnumerable<string> errors = null;
            
            switch (ex)
            {
                case BadRequestException badRequestEx:
                    statusCode = HttpStatusCode.BadRequest;
                    errorMessage = badRequestEx.Message;

                    break;
                case NotFoundException notFoundEx:
                    statusCode = HttpStatusCode.NotFound;
                    errorMessage = notFoundEx.Message;

                    break;
                case ValidationException validationEx:
                    statusCode = HttpStatusCode.BadRequest;
                    errorMessage = "Validation Error";
                    errors = validationEx.Errors;
                    
                    break;
                case NotSupportedMediaTypeException notSupportedEx:
                    statusCode = HttpStatusCode.UnsupportedMediaType;
                    errorMessage = notSupportedEx.Message;

                    break;
                case AlreadyExistsException alreadyExistsEx:
                    statusCode = HttpStatusCode.Conflict;
                    errorMessage = alreadyExistsEx.Message;

                    break;
                case OperationCanceledException operationCancelledEx:
                    statusCode = HttpStatusCode.BadRequest;
                    errorMessage = operationCancelledEx.Message;

                    break;
                default:
                    break;
            }

            var result = Response.CreateErrorResponse((int)statusCode, errorMessage, errors);

            httpContext.Response.StatusCode = (int)statusCode;

            return httpContext
                .Response
                .WriteAsync(JsonConvert.SerializeObject(result));
        }
    }
}
