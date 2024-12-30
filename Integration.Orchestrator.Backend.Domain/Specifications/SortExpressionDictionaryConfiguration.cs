using Integration.Orchestrator.Backend.Domain.Models.Configurador;
using MongoDB.Bson;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public class SortExpressionConfiguration<T>
    {

        public static Expression<Func<T, object>> ConvertOrderExpression<M>(Expression<Func<M, object>> orderExpr)
        {

            var parameter = Expression.Parameter(typeof(ServerResponseModel), "dto");
            MemberExpression memberExpression;

            if (orderExpr.Body is UnaryExpression unaryExpression)
            {
                memberExpression = unaryExpression.Operand as MemberExpression;
            }
            else
            {
                memberExpression = orderExpr.Body as MemberExpression;
            }

            if (memberExpression == null)
                throw new InvalidOperationException("La expresión proporcionada no es válida para una operación de orden.");

            var property = Expression.Property(parameter, memberExpression.Member.Name);

            return Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);


        }

        public static string GetPropertyName<T>(Expression<Func<T, object>> expression)
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
