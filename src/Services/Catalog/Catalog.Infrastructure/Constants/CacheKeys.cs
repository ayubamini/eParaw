namespace Catalog.Infrastructure.Constants
{
    public static class CacheKeys
    {
        public const string ProductPrefix = "product:";
        public const string CategoryPrefix = "category:";
        public const string ProductListPrefix = "products:";

        public static string GetProductKey(int productId) => $"{ProductPrefix}{productId}";
        public static string GetCategoryKey(int categoryId) => $"{CategoryPrefix}{categoryId}";
        public static string GetProductListKey(int pageNumber, int pageSize, string filters = "")
            => $"{ProductListPrefix}{pageNumber}:{pageSize}:{filters}";
        public static string GetCategoriesListKey(bool activeOnly) => $"categories:all:{activeOnly}";
    }
}
