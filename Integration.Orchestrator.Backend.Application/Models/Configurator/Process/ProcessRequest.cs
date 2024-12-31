using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Process
{
    [ExcludeFromCodeCoverage]
    public class ProcessRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid TypeId { get; set; }
        public Guid ConnectionId { get; set; }
        public Guid StatusId { get; set; }
        public IEnumerable<EntitiesRequest> Entities { get; set; } = Enumerable.Empty<EntitiesRequest>();
    }

    [ExcludeFromCodeCoverage]
    public class EntitiesRequest
    {
        public Guid Id { get; set; }
        public IEnumerable<PropertiesRequest> Properties { get; set; } = Enumerable.Empty<PropertiesRequest>();
        public IEnumerable<FilterRequest> Filters { get; set; } = Enumerable.Empty<FilterRequest>();
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
        public string Value { get; set; } = string.Empty;
    }
}
