using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Status
{
    [ExcludeFromCodeCoverage]
    public class StatusGetByIdRequest
    {
        public Guid Id { get; set; }
    }
}
