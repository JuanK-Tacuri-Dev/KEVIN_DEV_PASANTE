using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Adapter
{
    [ExcludeFromCodeCoverage]
    public class AdapterResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid TypeAdapterId { get; set; }
        public string Version { get; set; }
        public string TypeAdapterName { get; set; }
        public Guid StatusId { get; set; }
    }
}
