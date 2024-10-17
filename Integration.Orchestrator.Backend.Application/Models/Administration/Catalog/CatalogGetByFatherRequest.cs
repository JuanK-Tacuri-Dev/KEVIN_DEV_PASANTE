using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Catalog
{
    [ExcludeFromCodeCoverage]
    public class CatalogGetByFatherRequest
    {
        public int FatherCode { get; set; }
    }
}
