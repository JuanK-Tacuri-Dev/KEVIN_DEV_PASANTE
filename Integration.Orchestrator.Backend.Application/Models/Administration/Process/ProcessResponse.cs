using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Process
{
    [ExcludeFromCodeCoverage]
    public class ProcessResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid TypeId { get; set; }
        public Guid ConnectionId { get; set; }
        public Guid StatusId { get; set; }
        public List<EntityResponse> Entities { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class EntityResponse
    {
        public Guid Id { get; set; }
        public List<PropertiesResponse> Properties { get; set; }
        public List<FilterResponse> Filters { get; set; }
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
        public string Value { get; set; }
    }
}
