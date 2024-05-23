using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public interface ISpecification<T, M>
    {
        Expression<Func<T, bool>> Criteria { get; }
        Expression<Func<T, object>> OrderBy { get; }
        int Skip { get; }
        int Limit { get; }
        Expression<Func<T, bool>> BuildCriteria(M paginatedModel);
    }
}
