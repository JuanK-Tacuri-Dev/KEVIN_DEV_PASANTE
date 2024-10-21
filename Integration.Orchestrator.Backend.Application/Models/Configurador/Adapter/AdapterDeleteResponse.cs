using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Adapter
{
    [ExcludeFromCodeCoverage]
    public class AdapterDeleteResponse : ModelResponse<AdapterDelete>
    {
    }

    [ExcludeFromCodeCoverage]
    public class AdapterDelete
    {
        public Guid Id { get; set; }
    }
}
