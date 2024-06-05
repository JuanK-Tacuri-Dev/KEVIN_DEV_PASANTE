using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }
        
        int Skip { get; }
        int Limit { get; }
    }
}
