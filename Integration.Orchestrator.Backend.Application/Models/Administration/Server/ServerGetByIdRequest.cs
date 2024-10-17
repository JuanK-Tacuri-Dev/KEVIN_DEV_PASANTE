using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Server
{
    [ExcludeFromCodeCoverage]
    public class ServerGetByIdRequest
    {
        public Guid Id { get; set; }
    }
}
