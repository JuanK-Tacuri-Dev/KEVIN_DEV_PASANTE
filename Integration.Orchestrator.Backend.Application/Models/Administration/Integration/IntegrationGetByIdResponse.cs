using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Integration
{
    [ExcludeFromCodeCoverage]
    public class IntegrationGetByIdResponse : ModelResponse<IntegrationGetById>
    {
    }

    [ExcludeFromCodeCoverage]
    public class IntegrationGetById : IntegrationRequest
    {
        public Guid Id { get; set; }
    }
}
