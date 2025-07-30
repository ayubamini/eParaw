using Catalog.Domain.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using Catalog.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Catalog.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Database
            services.AddDbContext<CatalogContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(CatalogContext).Assembly.FullName)));

            // Redis Cache
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis") ?? "localhost:6379";
                options.InstanceName = "eparaw:";
            });

            // Cache Service
            services.AddSingleton<ICacheService, RedisCacheService>();

            // Repositories
            services.AddScoped<ProductRepository>();
            services.AddScoped<CategoryRepository>();

            // Decorated repositories with caching
            services.AddScoped<IProductRepository>(provider =>
            {
                var productRepo = provider.GetRequiredService<ProductRepository>();
                var cacheService = provider.GetRequiredService<ICacheService>();
                var logger = provider.GetRequiredService<ILogger<CachedProductRepository>>();

                return new CachedProductRepository(productRepo, cacheService, logger);
            });

            services.AddScoped<ICategoryRepository>(provider =>
            {
                var categoryRepo = provider.GetRequiredService<CategoryRepository>();
                var cacheService = provider.GetRequiredService<ICacheService>();
                var logger = provider.GetRequiredService<ILogger<CachedCategoryRepository>>();

                return new CachedCategoryRepository(categoryRepo, cacheService, logger);
            });

            return services;
        }
    }
}
