using Notino.Domain.Contracts.ResponseConversion.Converters;

namespace Notino.Domain.Contracts.ResponseConversion.Factory
{
    public interface IResponseConverterFactory
    {
        IResponseConverter Create(string responseType);
    }
}
