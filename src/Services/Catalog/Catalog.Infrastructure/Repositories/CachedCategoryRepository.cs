using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Catalog.Infrastructure.Constants;
using Catalog.Infrastructure.Services;
using Microsoft.Extensions.Logging;

namespace Catalog.Infrastructure.Repositories
{
    public class CachedCategoryRepository : ICategoryRepository
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly ICacheService cacheService;
        private readonly ILogger<CachedCategoryRepository> logger;
        private readonly TimeSpan _cacheExpiry = TimeSpan.FromMinutes(30); // Categories change less frequently

        public CachedCategoryRepository(ICategoryRepository categoryRepository, ICacheService cacheService, ILogger<CachedCategoryRepository> logger)
        {
            this.categoryRepository = categoryRepository;
            this.cacheService = cacheService;
            this.logger = logger;
        }
        public async Task<Category> AddAsync(Category category)
        {
            var result = await categoryRepository.AddAsync(category);

            // Invalidate list caches
            await InvalidateCategoryCaches();

            logger.LogInformation("Category {CategoryId} added and cache invalidated", result.Id);

            return result;
        }

        public async Task DeleteAsync(Category category)
        {
            await categoryRepository.DeleteAsync(category);

            // Remove from cache
            await cacheService.RemoveAsync(CacheKeys.GetCategoryKey(category.Id));
            await cacheService.RemoveAsync($"{CacheKeys.CategoryPrefix}name:{category.Name}");
            await InvalidateCategoryCaches();

            logger.LogInformation("Category {CategoryId} deleted and cache invalidated", category.Id);
        }



        public async Task<bool> ExistsAsync(int id)
        {
            // Don't cache existence checks
            return await categoryRepository.ExistsAsync(id);
        }

        public async Task<IEnumerable<Category>> GetActiveAsync()
        {
            var cacheKey = CacheKeys.GetCategoriesListKey(true);

            return await cacheService.GetOrCreateAsync(
                cacheKey,
                async () => await categoryRepository.GetActiveAsync(),
                _cacheExpiry);
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var cacheKey = CacheKeys.GetCategoriesListKey(false);

            return await cacheService.GetOrCreateAsync(
                cacheKey,
                async () => await categoryRepository.GetAllAsync(),
                _cacheExpiry);
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            var cacheKey = CacheKeys.GetCategoryKey(id);

            return await cacheService.GetOrCreateAsync(
                cacheKey,
                async () => await categoryRepository.GetByIdAsync(id),
                _cacheExpiry);
        }

        public async Task<Category> GetByNameAsync(string name)
        {
            var cacheKey = $"{CacheKeys.CategoryPrefix}name:{name}";

            return await cacheService.GetOrCreateAsync(
                cacheKey,
                async () => await categoryRepository.GetByNameAsync(name),
                _cacheExpiry);
        }

        public async Task UpdateAsync(Category category)
        {
            await categoryRepository.UpdateAsync(category);

            // Remove from cache
            await cacheService.RemoveAsync(CacheKeys.GetCategoryKey(category.Id));
            await cacheService.RemoveAsync($"{CacheKeys.CategoryPrefix}name:{category.Name}");
            await InvalidateCategoryCaches();

            logger.LogInformation("Category {CategoryId} updated and cache invalidated", category.Id);
        }

        private async Task InvalidateCategoryCaches()
        {
            await cacheService.RemoveAsync(CacheKeys.GetCategoriesListKey(true));
            await cacheService.RemoveAsync(CacheKeys.GetCategoriesListKey(false));
        }
    }
}
