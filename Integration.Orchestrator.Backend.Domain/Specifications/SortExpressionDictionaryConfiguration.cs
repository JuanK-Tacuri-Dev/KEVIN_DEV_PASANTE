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
    public static class BsonDocumentExtensions
    {
        public static T GetValueOrDefault<T>(this BsonDocument doc, string key, T defaultValue)
        {
            if (doc.TryGetValue(key, out var value) && value != null && !value.IsBsonNull)
            {
                try
                {
                    if (typeof(T) == typeof(Guid) && value.IsString)
                    {
                        return (T)(object)Guid.Parse(value.AsString);
                    }

                    if (typeof(T) == typeof(string)) return (T)(object)value.AsString;
                    if (typeof(T) == typeof(int)) return (T)(object)value.AsInt32;
                    if (typeof(T) == typeof(long)) return (T)(object)value.AsInt64;
                    if (typeof(T) == typeof(bool)) return (T)(object)value.AsBoolean;
                    if (typeof(T) == typeof(DateTime)) return (T)(object)value.ToUniversalTime();

                    return (T)Convert.ChangeType(value.RawValue, typeof(T));
                }
                catch
                {
                    return defaultValue;
                }
            }

            return defaultValue;
        }

        public static string GetNestedValueOrDefault(this BsonDocument doc, string arrayKey, string nestedKey)
        {
            return doc.TryGetValue(arrayKey, out var array) && array.IsBsonArray
                ? array.AsBsonArray.FirstOrDefault()?.AsBsonDocument?[nestedKey]?.AsString ?? string.Empty
                : string.Empty;
        }

        public static List<T> TryGetArray<T>(this BsonDocument doc, string key, Func<BsonValue, T> mapFunction)
        {
            return doc.TryGetValue(key, out var value) && value.IsBsonArray
                ? value.AsBsonArray.Select(mapFunction).ToList()
                : new List<T>();
        }
    }

}
