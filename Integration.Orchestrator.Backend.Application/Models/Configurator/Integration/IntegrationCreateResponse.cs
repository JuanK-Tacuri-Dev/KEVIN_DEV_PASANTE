using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Integration
{
    [ExcludeFromCodeCoverage]
    public class IntegrationCreateResponse : ModelResponse<IntegrationCreate>
    {
    }

    [ExcludeFromCodeCoverage]
    public class IntegrationCreate : IntegrationRequest
    {
        public Guid Id { get; set; }
    }
}
