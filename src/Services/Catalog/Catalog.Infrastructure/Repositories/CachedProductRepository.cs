using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Catalog.Infrastructure.Constants;
using Catalog.Infrastructure.Services;
using Microsoft.Extensions.Logging;

namespace Catalog.Infrastructure.Repositories
{
    public class CachedProductRepository : IProductRepository
    {
        private readonly IProductRepository productRepository;
        private readonly ICacheService cacheService;
        private readonly ILogger<CachedProductRepository> logger;
        private readonly TimeSpan _cacheExpiry = TimeSpan.FromMinutes(10);

        public CachedProductRepository(IProductRepository productRepository, ICacheService cacheService, ILogger<CachedProductRepository> logger)
        {
            this.productRepository = productRepository;
            this.cacheService = cacheService;
            this.logger = logger;
        }


        public async Task<Product> AddAsync(Product product)
        {
            var result = await productRepository.AddAsync(product);

            // Invalidate related caches
            await InvalidateProductCaches(product);

            logger.LogInformation("Product {ProductId} added and cache invalidated", result.Id);

            return result;
        }

        public async Task DeleteAsync(Product product)
        {
            await productRepository.DeleteAsync(product);

            // Remove from cache
            await cacheService.RemoveAsync(CacheKeys.GetProductKey(product.Id));
            await cacheService.RemoveAsync($"{CacheKeys.ProductPrefix}sku:{product.Sku}");
            await InvalidateProductCaches(product);

            logger.LogInformation("Product {ProductId} deleted and cache invalidated", product.Id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            // Don't cache existence checks
            return await productRepository.ExistsAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await productRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
        {
            var cacheKey = $"{CacheKeys.ProductPrefix}category:{categoryId}";

            return await cacheService.GetOrCreateAsync(
                cacheKey,
                async () => await productRepository.GetByCategoryIdAsync(categoryId),
                _cacheExpiry);
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var cacheKey = CacheKeys.GetProductKey(id);

            return await cacheService.GetOrCreateAsync(cacheKey, async () => await productRepository.GetByIdAsync(id), _cacheExpiry);
        }

        public async Task<Product> GetBySkuAsync(string sku)
        {
            var cacheKey = $"{CacheKeys.ProductPrefix}sku:{sku}";

            return await this.cacheService.GetOrCreateAsync(
                cacheKey,
                async () => await this.productRepository.GetBySkuAsync(sku),
                _cacheExpiry);
        }

        public async Task<(IEnumerable<Product> Products, int TotalCount)> GetPagedAsync(
        int pageNumber, int pageSize, string searchTerm = null,
        int? categoryId = null, decimal? minPrice = null,
        decimal? maxPrice = null, bool? inStockOnly = null)
        {
            // Create cache key based on filter parameters
            var filters = $"{searchTerm}_{categoryId}_{minPrice}_{maxPrice}_{inStockOnly}";
            var cacheKey = CacheKeys.GetProductListKey(pageNumber, pageSize, filters);

            // For complex queries, we might cache for shorter duration
            var result = await cacheService.GetOrCreateAsync(
                cacheKey,
                async () => await productRepository.GetPagedAsync(
                    pageNumber, pageSize, searchTerm, categoryId,
                    minPrice, maxPrice, inStockOnly),
                TimeSpan.FromMinutes(5));

            return result;
        }

        public async Task<bool> SkuExistsAsync(string sku)
        {
            // Don't cache existence checks
            return await productRepository.SkuExistsAsync(sku);
        }

        public async Task UpdateAsync(Product product)
        {
            await productRepository.UpdateAsync(product);

            // Remove from cache to ensure fresh data
            await cacheService.RemoveAsync(CacheKeys.GetProductKey(product.Id));
            await cacheService.RemoveAsync($"{CacheKeys.ProductPrefix}sku:{product.Sku}");
            await InvalidateProductCaches(product);

            logger.LogInformation("Product {ProductId} updated and cache invalidated", product.Id);
        }

        private async Task InvalidateProductCaches(Product product)
        {
            // Invalidate category-specific caches
            await cacheService.RemoveAsync($"{CacheKeys.ProductPrefix}category:{product.CategoryId}");

            // In a real application, you might want to invalidate all paginated results
            // For now, we'll let them expire naturally
        }
    }
}
