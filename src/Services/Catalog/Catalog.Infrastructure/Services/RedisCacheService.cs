using Microsoft.CodeAnalysis.Operations;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Catalog.Infrastructure.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly string _instanceName = "eparaw:catalog:";

        public RedisCacheService(IDistributedCache distributedCache)
        {
            distributedCache = distributedCache;
        }

        public async Task<bool> ExistAsync(string key)
        {
            var value = await _distributedCache.GetAsync($"{_instanceName}{key}");

            return value != null;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _distributedCache.GetStringAsync($"{_instanceName}{key}");

            if (string.IsNullOrEmpty(value))
                return default(T);

            return JsonSerializer.Deserialize<T>(value);
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiry = null)
        {
            var cachedValue = await GetAsync<T>(key);

            if (cachedValue != null)
                return cachedValue;

            var value = await factory();

            if(value != null)
                await SetAsync(key, value, expiry);

            return value;
        }

        public async Task<string> GetStringAsync(string key)
        {
            return await _distributedCache.GetStringAsync($"{_instanceName}{key}");
        }

        public async Task RemoveAsync(string key)
        {
            await _distributedCache.RemoveAsync($"{_instanceName}{key}");
        }

        public async Task RemoveByPrefixAsync(string prefixKey)
        {
            // Note: This is a simplified version. In production, you might want to use Redis SCAN command
            // For now, we'll implement specific invalidation methods
            await Task.CompletedTask;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var options = new DistributedCacheEntryOptions();

            if (expiry.HasValue)
                options.SetAbsoluteExpiration(expiry.Value);
            else
                options.SetAbsoluteExpiration(TimeSpan.FromMinutes(30)); //default value

            var serializeValue = JsonSerializer.Serialize(value);

            await _distributedCache.SetStringAsync($"{_instanceName}{key}", serializeValue, options);
        }

        public async Task SetStringAsync(string key, string value, TimeSpan? expiry = null)
        {
            var options = new DistributedCacheEntryOptions();

            if (expiry.HasValue)
                options.SetAbsoluteExpiration(expiry.Value);
            else
                options.SetAbsoluteExpiration(TimeSpan.FromMinutes(30)); //default value

            await _distributedCache.SetStringAsync($"{_instanceName}{key}", value, options);
        }
    }
}
