using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Property
{
    [ExcludeFromCodeCoverage]
    public class PropertyResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public Guid TypeId { get; set; }
        public string TypePropertyName { get; set; } = string.Empty;
        public Guid EntityId { get; set; }
        public string EntityName { get; set; } = string.Empty;
        public Guid StatusId { get; set; }
    }
}
