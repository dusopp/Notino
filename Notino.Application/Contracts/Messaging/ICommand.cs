using MediatR;

namespace Notino.Application.Contracts.Messaging
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}
