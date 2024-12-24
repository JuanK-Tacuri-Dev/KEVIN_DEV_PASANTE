using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Catalog
{
    [ExcludeFromCodeCoverage]
    public class CatalogGetByIdRequest
    {
        public Guid Id { get; set; }
    }
}
