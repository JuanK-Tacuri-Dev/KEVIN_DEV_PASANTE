using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Property
{
    [ExcludeFromCodeCoverage]
    public class PropertyResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Guid TypeId { get; set; }
        public string typePropertyName { get; set; }
        public Guid EntityId { get; set; }
        public string entityName { get; set; }
        public Guid StatusId { get; set; }
    }
}
