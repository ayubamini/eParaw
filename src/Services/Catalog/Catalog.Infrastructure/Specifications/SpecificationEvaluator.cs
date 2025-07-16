using Catalog.Domain.Common;
using Catalog.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Specifications
{
    public class SpecificationEvaluator<TEntity> where TEntity : EntityBase
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            var query = inputQuery;

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            query = spec.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}
