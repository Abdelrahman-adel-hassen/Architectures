namespace LearningJourney.Application.Common.Behaviors.Caching
{
    public class CommandCacheBehavior<TRequest, TResponse>(IDistributedCache cache) : IPipelineBehavior<TRequest, TResponse>
      where TRequest : ICacheCommand
    {
        private readonly IDistributedCache _cache = cache;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = await next();

            foreach (var key in request.CacheKeys)
            {
                await _cache.RemoveAsync(key, cancellationToken);
            }

            return response;
        }
    }
}
