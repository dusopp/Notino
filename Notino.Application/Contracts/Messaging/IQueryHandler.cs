using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notino.Application.Contracts.Messaging
{
    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        where TQuery : IQueryRequest<TResponse>
    {
    }
}
