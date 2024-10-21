using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Server
{
    [ExcludeFromCodeCoverage]
    public class ServerGetByIdRequest
    {
        public Guid Id { get; set; }
    }
}
