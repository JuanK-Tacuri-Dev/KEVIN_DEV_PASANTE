using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Catalog
{
    [ExcludeFromCodeCoverage]
    public class CatalogGetByFatherResponse : ModelResponse<IEnumerable<CatalogGetByType>>
    {
    }

    [ExcludeFromCodeCoverage]
    public class CatalogGetByType : CatalogResponse
    {
    }
}
