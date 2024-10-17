using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Entities
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
