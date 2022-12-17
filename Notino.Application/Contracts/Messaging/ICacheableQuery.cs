using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notino.Application.Contracts.Messaging
{
    public interface ICacheableQuery<TResponse>: IQueryRequest<TResponse>
    {
        bool BypassCache { get; }
        string CacheKey { get; }
        TimeSpan? SlidingExpiration { get; }
    }
}
