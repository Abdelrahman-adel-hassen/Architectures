namespace LearningJourney.Application.Common.Behaviors.Caching;

public class QueryCachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ICacheProvider _cache;

    public QueryCachingBehavior(ICacheProvider cache)
    {
        _cache = cache;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is not ICacheableQuery cacheable)
            return await next();

        if (string.IsNullOrWhiteSpace(cacheable.CacheKey))
            return await next();

        var (found, cached) = await _cache.TryGetAsync<TResponse>(cacheable.CacheKey);
        if (found)
            return cached;

        var response = await next();

        if (response is null)
            return response;

        await _cache.SetAsync(cacheable.CacheKey, response, cacheable.Expiration ?? TimeSpan.FromMinutes(1));

        return response;
    }
}
