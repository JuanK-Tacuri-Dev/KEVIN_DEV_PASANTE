using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Integration.Orchestrator.Backend.Domain.Entities
{
    public class Entity<T>
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        [Key]
        public virtual T id { get; set; }

    }
}
