using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Integration
{
    [ExcludeFromCodeCoverage]
    public class IntegrationDeleteResponse : ModelResponse<IntegrationDelete>
    {
    }

    [ExcludeFromCodeCoverage]
    public class IntegrationDelete 
    {
        public Guid Id { get; set; }
    }
}
