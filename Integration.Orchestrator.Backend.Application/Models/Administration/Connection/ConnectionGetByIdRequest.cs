using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Connection
{
    [ExcludeFromCodeCoverage]
    public class ConnectionGetByIdRequest
    {
        public Guid Id { get; set; }
    }
}
