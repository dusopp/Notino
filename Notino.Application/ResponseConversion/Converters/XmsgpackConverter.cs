using MessagePack;
using Notino.Domain.Contracts.ResponseConversion.Converters;
using System.Text;

namespace Notino.Application.ResponseConversion.Converters
{
    public class XmsgpackConverter : IResponseConverter
    {
        public string Convert(string json)
        {
            var messagePack = MessagePackSerializer.ConvertFromJson(json);
            return Encoding.UTF8.GetString(messagePack);
        }
    }
}
