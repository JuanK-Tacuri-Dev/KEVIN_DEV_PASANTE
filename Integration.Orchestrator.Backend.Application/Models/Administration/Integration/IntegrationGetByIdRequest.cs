using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Integration
{
    [ExcludeFromCodeCoverage]
    public class IntegrationGetByIdRequest
    {
        public Guid Id { get; set; }
    }
}
