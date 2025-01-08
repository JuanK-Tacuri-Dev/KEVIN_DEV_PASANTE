using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Text.Json;

namespace Integration.Orchestrator.Backend.Infrastructure.Services
{
    [ExcludeFromCodeCoverage]
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

        public static SortDefinition<BsonDocument> GetSortDefinition(string? orderByField, bool isAscending, Dictionary<string, string>  sortMapping, string sortDefinitionDefault= "updated_at")
        {
            var sortDefinitionBuilder = Builders<BsonDocument>.Sort;

            if (orderByField == null)
            {
                return sortDefinitionBuilder.Ascending(sortDefinitionDefault);
            }
            var sortField = sortMapping.ContainsKey(orderByField) ? sortMapping[orderByField] : orderByField;
            return isAscending
                ? sortDefinitionBuilder.Ascending(sortField)
                : sortDefinitionBuilder.Descending(sortField);
        }

        public static FilterDefinition<BsonDocument> GetFilterDefinition(Expression<Func<BsonDocument, bool>>? criteria,Dictionary<string, object> additionalFilters,Dictionary<string, string> fieldMapping)
        {
            var filterBuilder = Builders<BsonDocument>.Filter;

            // Inicializar el filtro con un filtro vacío o el criterio principal
            var filter = criteria != null ? filterBuilder.Where(criteria) : filterBuilder.Empty;

            // Agregar filtros adicionales dinámicamente
            foreach (var filterItem in additionalFilters)
            {
                // Obtener el nombre del campo mapeado
                string mappedField = fieldMapping.ContainsKey(filterItem.Key) ? fieldMapping[filterItem.Key] : filterItem.Key;

                // Agregar el filtro al conjunto de filtros
                filter = filter & filterBuilder.Eq(mappedField, filterItem.Value);
            }

            return filter;
        }

        public static object ProcessFilterValue(string mappedField, object filterValue)
        {
            if (filterValue is IEnumerable<object> values && values.Any())
            {
                var firstValue = values.First();

                if (firstValue is JsonElement jsonElement)
                {
                    // Detectar tipo en JsonElement
                    return jsonElement.ValueKind switch
                    {
                        JsonValueKind.String => values.Cast<JsonElement>().Select(v => v.GetString()).ToList(),
                        JsonValueKind.Number => values.Cast<JsonElement>().Select(v => v.GetInt32()).ToList(),
                        JsonValueKind.True or JsonValueKind.False => values.Cast<JsonElement>().Select(v => v.GetBoolean()).ToList(),
                        _ => throw new InvalidOperationException($"Unsupported JSON ValueKind {jsonElement.ValueKind}")
                    };
                }
                else
                {
                    // Manejo estándar de otros tipos
                    Type valueType = firstValue.GetType();

                    return valueType switch
                    {
                        Type _ when valueType == typeof(string) => values.Cast<string>().ToList(),
                        Type _ when valueType == typeof(bool) => values.Cast<bool>().ToList(),
                        Type _ when valueType == typeof(int) => values.Cast<int>().ToList(),
                        _ => throw new InvalidOperationException($"Unsupported filter value type {valueType}")
                    };
                }
            }
            else if (filterValue is JsonElement jsonElement)
            {
                // Manejar valor único JsonElement
                return jsonElement.ValueKind switch
                {
                    JsonValueKind.String => jsonElement.GetString(),
                    JsonValueKind.Number => jsonElement.GetInt32(), // Usa el método adecuado (GetDecimal, GetDouble, etc.)
                    JsonValueKind.True or JsonValueKind.False => jsonElement.GetBoolean(),
                    _ => throw new InvalidOperationException($"Unsupported JSON ValueKind {jsonElement.ValueKind}")
                };
            }
            else
            {
                // Manejo estándar para valores únicos no JsonElement
                Type valueType = filterValue?.GetType();

                return valueType switch
                {
                    Type _ when valueType == typeof(string) => (string)filterValue,
                    Type _ when valueType == typeof(bool) => (bool)filterValue,
                    Type _ when valueType == typeof(int) => (int)filterValue,
                    _ => throw new InvalidOperationException($"Unsupported filter value type {valueType}")
                };
            }
        }


    }
}
