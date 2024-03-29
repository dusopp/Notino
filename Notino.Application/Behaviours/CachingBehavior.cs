﻿using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Notino.Application.Contracts.Messaging;
using Notino.Application.DTOs.Common;
using Notino.Application.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notino.Application.Behaviours
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class, ICacheableQuery<TResponse>
        where TResponse : class, IRawResponseDto
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger _logger;
        private readonly CacheSettings _settings;
        public CachingBehavior(IDistributedCache cache, ILogger<TResponse> logger, IOptions<CacheSettings> settings)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
            _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse response;
            if (request.BypassCache)
                return await next();

            async Task<TResponse> GetResponseAndAddToCache()
            {
                response = await next();

                var slidingExpiration = 
                    request.SlidingExpiration == null ? 
                    TimeSpan.FromHours(_settings.SlidingExpiration) : 
                    request.SlidingExpiration;

                var options = new DistributedCacheEntryOptions { 
                    SlidingExpiration = slidingExpiration 
                };
                
                var serializedData = Encoding
                    .Default
                    .GetBytes(JsonConvert.SerializeObject(response));

                await _cache
                    .SetAsync(request.CacheKey, serializedData, options, cancellationToken);
                
                return response;
            }

            var cachedResponse = await _cache.
                GetAsync(request.CacheKey, cancellationToken);

            if (cachedResponse != null)
            {
                response = JsonConvert
                    .DeserializeObject<TResponse>(Encoding.Default.GetString(cachedResponse));

                _logger.LogInformation($"Fetched from Cache -> '{request.CacheKey}'.");
            }
            else
            {
                response = await GetResponseAndAddToCache();
                _logger.LogInformation($"Added to Cache -> '{request.CacheKey}'.");
            }

            return response;
        }
    }
}
