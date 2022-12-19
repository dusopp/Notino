using Notino.Application.Contracts.ResponseConversion.Converters;

namespace Notino.Application.Contracts.ResponseConversion.Factory
{
    public interface IResponseConverterFactory
    {
        IResponseConverter Create(string responseType);
    }
}
