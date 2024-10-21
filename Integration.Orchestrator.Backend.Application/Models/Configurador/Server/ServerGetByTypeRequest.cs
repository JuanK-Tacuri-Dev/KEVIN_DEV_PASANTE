using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Server
{
    [ExcludeFromCodeCoverage]
    public class ServerGetByTypeRequest
    {
        public Guid Type { get; set; }
    }
}
