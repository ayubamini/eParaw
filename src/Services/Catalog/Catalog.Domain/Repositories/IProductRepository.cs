// Repositories/IProductRepository.cs
namespace Catalog.Domain.Repositories;

using Catalog.Domain.Entities;

public interface IProductRepository
{
    Task<Product> GetByIdAsync(int id);
    Task<Product> GetBySkuAsync(string sku);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);
    Task<(IEnumerable<Product> Products, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string searchTerm = null,
        int? categoryId = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        bool? inStockOnly = null);
    Task<Product> AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
    Task<bool> ExistsAsync(int id);
    Task<bool> SkuExistsAsync(string sku);
}