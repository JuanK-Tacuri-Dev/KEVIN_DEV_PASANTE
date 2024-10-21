using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Status
{
    [ExcludeFromCodeCoverage]
    public class StatusDeleteResponse : ModelResponse<StatusDelete>
    {
    }

    [ExcludeFromCodeCoverage]
    public class StatusDelete
    {
        public Guid Id { get; set; }
    }
}
