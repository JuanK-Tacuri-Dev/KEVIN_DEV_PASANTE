using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Integration
{
    [ExcludeFromCodeCoverage]
    public class IntegrationUpdateResponse : ModelResponse<IntegrationUpdate>
    {
    }

    [ExcludeFromCodeCoverage]
    public class IntegrationUpdate : IntegrationRequest
    {
        public Guid Id { get; set; }
    }
}
