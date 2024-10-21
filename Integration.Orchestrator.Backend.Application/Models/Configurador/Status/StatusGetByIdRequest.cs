using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Status
{
    [ExcludeFromCodeCoverage]
    public class StatusGetByIdRequest
    {
        public Guid Id { get; set; }
    }
}
