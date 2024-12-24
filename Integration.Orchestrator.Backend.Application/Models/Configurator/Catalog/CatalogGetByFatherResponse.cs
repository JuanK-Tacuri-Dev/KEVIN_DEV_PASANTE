using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Catalog
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
