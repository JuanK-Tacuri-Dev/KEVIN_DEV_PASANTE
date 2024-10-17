using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Catalog
{
    [ExcludeFromCodeCoverage]
    public class CatalogGetByIdRequest
    {
        public Guid Id { get; set; }
    }
}
