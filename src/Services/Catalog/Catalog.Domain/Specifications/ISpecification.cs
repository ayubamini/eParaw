// Specifications/ISpecification.cs
namespace Catalog.Domain.Specifications;

using System.Linq.Expressions;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
    List<Expression<Func<T, object>>> Includes { get; }
    List<string> IncludeStrings { get; }
}

