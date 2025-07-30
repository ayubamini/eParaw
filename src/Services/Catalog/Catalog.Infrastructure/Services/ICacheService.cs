using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Catalog.Infrastructure.Services
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string key);
        Task<string> GetStringAsync(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
        Task SetStringAsync(string key, string value, TimeSpan? expiry = null);
        Task<bool> ExistAsync(string key);
        Task RemoveAsync(string key);
        Task RemoveByPrefixAsync(string prefixKey);
        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiry = null);
    }
}
