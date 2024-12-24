using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Process
{
    [ExcludeFromCodeCoverage]
    public class ProcessResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TypeProcessName { get; set; } = string.Empty;
        public string ConnectionName { get; set; } = string.Empty;
        public Guid TypeId { get; set; }
        public Guid ConnectionId { get; set; }
        public Guid StatusId { get; set; }
        public IEnumerable<EntityResponse> Entities { get; set; } = Enumerable.Empty<EntityResponse>();
    }

    [ExcludeFromCodeCoverage]
    public class EntityResponse
    {
        public Guid Id { get; set; }
        public IEnumerable<PropertiesResponse> Properties { get; set; } = Enumerable.Empty<PropertiesResponse>();
        public IEnumerable<FilterResponse> Filters { get; set; } = Enumerable.Empty<FilterResponse>();
    }


    [ExcludeFromCodeCoverage]
    public class PropertiesResponse
    {
        public Guid Id { get; set; }
        public Guid InternalStatusId { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class FilterResponse
    {
        public Guid PropertyId { get; set; }
        public Guid OperatorId { get; set; }
        public string Value { get; set; } = string.Empty;
    }
}
