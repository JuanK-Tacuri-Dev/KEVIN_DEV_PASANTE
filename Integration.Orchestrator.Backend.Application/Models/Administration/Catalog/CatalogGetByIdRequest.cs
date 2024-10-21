using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Catalog
{
    [ExcludeFromCodeCoverage]
    public class CatalogGetByIdRequest
    {
        public Guid Id { get; set; }
    }
}
