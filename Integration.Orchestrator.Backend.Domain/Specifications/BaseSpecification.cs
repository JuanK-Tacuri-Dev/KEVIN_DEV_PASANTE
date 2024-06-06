using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public static class BaseSpecification<T>
    {
        public static Expression<Func<T, bool>> GetAll()
        {
            return x => true;
        }

        public static Expression<Func<T, bool>> GetByUuid(Expression<Func<T, Guid>> propertySelector, Guid uuid)
        {
            var parameter = propertySelector.Parameters[0];
            var body = Expression.Equal(propertySelector.Body, Expression.Constant(uuid));
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }
}
