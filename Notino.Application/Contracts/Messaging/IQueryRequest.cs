using MediatR;

namespace Notino.Application.Contracts.Messaging
{
    public interface IQueryRequest<TResponse> : IRequest<TResponse>
    {
        public string AcceptHeader { get; set; }
    }
}
