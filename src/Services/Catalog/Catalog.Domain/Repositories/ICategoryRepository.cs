using Catalog.Domain.Entities;

namespace Catalog.Domain.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> GetByIdAsync(int id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<IEnumerable<Category>> GetActiveAsync();
        Task<Category> GetByNameAsync(string name);
        Task<Category> AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(Category category);
        Task<bool> ExistsAsync(int id);
    }

}

