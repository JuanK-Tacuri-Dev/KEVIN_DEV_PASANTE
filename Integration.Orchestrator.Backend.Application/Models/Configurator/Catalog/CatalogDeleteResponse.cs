using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Catalog
{
    [ExcludeFromCodeCoverage]
    public class CatalogDeleteResponse : ModelResponse<CatalogDelete>
    {
    }

    [ExcludeFromCodeCoverage]
    public class CatalogDelete
    {
        public Guid Id { get; set;}
    }
}
