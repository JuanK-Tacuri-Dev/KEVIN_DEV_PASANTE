using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<LookupSpecification<T>> Includes { get; }
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }
        Dictionary<string, object> AdditionalFilters { get; }
        int Skip { get; }
        int Limit { get; }
    }
}
