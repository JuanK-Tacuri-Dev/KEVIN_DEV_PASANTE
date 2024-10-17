using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Server
{
    [ExcludeFromCodeCoverage]
    public class ServerDeleteResponse : ModelResponse<ServerDelete>
    {
    }

    [ExcludeFromCodeCoverage]
    public class ServerDelete
    {
        public Guid Id { get; set; }
    }
}
