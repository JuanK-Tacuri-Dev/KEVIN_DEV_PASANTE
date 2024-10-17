using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Process
{
    [ExcludeFromCodeCoverage]
    public class ProcessRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid TypeId { get; set; }
        public Guid ConnectionId { get; set; }
        public Guid StatusId { get; set; }
        public List<EntitiesRequest> Entities { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class EntitiesRequest
    {
        public Guid Id { get; set; }
        public List<PropertiesRequest> Properties { get; set; }
        public List<FilterRequest> Filters { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class PropertiesRequest
    {
        public Guid Id { get; set; }
        public Guid InternalStatusId { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class FilterRequest
    {
        public Guid PropertyId { get; set; }
        public Guid OperatorId { get; set; }
        public string Value { get; set; }
    }
}
