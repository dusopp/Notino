using Microsoft.Extensions.Logging;
using Notino.Application.Exceptions;
using Notino.CrossCutting.Extensions;
using Notino.Domain.Contracts.ResponseConversion.Converters;
using Notino.Domain.Contracts.ResponseConversion.Factory;
using System;

namespace Notino.Application.ResponseConversion.Factory
{
    public class ResponseConverterFactory : IResponseConverterFactory
    {
        private readonly ILogger<ResponseConverterFactory> _logger;

        public ResponseConverterFactory(ILogger<ResponseConverterFactory> logger)
        {
           _logger = logger;
        }

        public IResponseConverter Create(string responseType)
        {
            string converterType = GetConverterName(responseType);
            string converterName = $"Notino.Application.ResponseConversion.Converters.{converterType}Converter";
            
            try
            {
                return (IResponseConverter)Activator.CreateInstance(
                   Type.GetType(converterName)
                );
            }
            catch 
            {
                throw new NotSupportedMediaTypeException(responseType);
            }
        }

        private string GetConverterName(string responseType)
        {
            responseType = responseType.Replace("-", "");
            var indexOfType = responseType.IndexOf('/');
            var converterName = responseType.Substring(indexOfType + 1, responseType.Length - 1 - indexOfType);
            
            return converterName.FirstCharToUpper();
        }
    }
}
