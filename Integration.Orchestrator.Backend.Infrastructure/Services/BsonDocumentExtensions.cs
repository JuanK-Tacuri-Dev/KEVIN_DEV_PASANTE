using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Orchestrator.Backend.Infrastructure.Services
{
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
    }
}
