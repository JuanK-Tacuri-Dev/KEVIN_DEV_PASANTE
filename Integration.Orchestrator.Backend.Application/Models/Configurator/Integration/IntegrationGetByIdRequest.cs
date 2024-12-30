using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Integration
{
    [ExcludeFromCodeCoverage]
    public class IntegrationGetByIdRequest
    {
        public Guid Id { get; set; }
    }
}
