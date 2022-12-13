using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notino.Application.Contracts.Messaging
{
    public interface IPipelineHandler<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IQueryRequest<TResponse>
    {
    }
}
