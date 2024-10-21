using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Entities
{
    [ExcludeFromCodeCoverage]
    public class EntitiesDeleteResponse : ModelResponse<EntitiesDelete>
    {
    }

    [ExcludeFromCodeCoverage]
    public class EntitiesDelete
    {
        public Guid Id { get; set; }
    }
}
