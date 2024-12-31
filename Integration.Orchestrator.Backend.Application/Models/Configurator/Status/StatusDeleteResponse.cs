using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Status
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
