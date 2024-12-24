using Integration.Orchestrator.Backend.Domain.Models.Configurator;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{

    public class SortExpressionConfiguration<T>
    {

        public static string GetPropertyName<P>(Expression<Func<P, object>> expression)
        {
            if (expression.Body is MemberExpression member)
            {
                return member.Member.Name;
            }

            if (expression.Body is UnaryExpression unaryExpression && unaryExpression.Operand is MemberExpression memberExpr)
            {
                return memberExpr.Member.Name;
            }

            throw new ArgumentException("Invalid expression");
        }


    }


}
