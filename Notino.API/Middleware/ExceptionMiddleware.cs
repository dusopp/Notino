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
        private readonly ILogger<ExceptionMiddleware> logger;

               
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error: {httpContext.Request.Path}");

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {            
            httpContext.Response.ContentType = "application/json";
            HttpStatusCode statusCode = HttpStatusCode.ServiceUnavailable;
            string errorMessage = "Service not available";
            IEnumerable<string> errors = null;
            
            switch (ex)
            {
                case BadRequestException badRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    errorMessage = badRequestException.Message;

                    break;
                case NotFoundException notFound:
                    statusCode = HttpStatusCode.NotFound;
                    errorMessage = notFound.Message;

                    break;
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    errorMessage = "Validation Error";
                    errors = validationException.Errors;
                    
                    break;
                case NotSupportedMediaTypeException notSupported:
                    statusCode = HttpStatusCode.UnsupportedMediaType;
                    errorMessage = notSupported.Message;
                    
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
