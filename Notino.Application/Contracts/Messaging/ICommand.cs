using MediatR;

namespace Notino.Application.Contracts.Messaging
{
    public interface ICommand<TResponse> : IRequest<TResponse>
    {
    }
}
