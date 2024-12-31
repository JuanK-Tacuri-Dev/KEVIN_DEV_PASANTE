using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Process
{
    [ExcludeFromCodeCoverage]
    public class ProcessGetByTypeRequest
    {
        public Guid TypeId { get; set; }
    }
}
