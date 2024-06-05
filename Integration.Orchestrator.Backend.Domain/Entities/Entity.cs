using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Integration.Orchestrator.Backend.Domain.Entities
{
    public class Entity<T>
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public T id { get; set; }
    }
}
