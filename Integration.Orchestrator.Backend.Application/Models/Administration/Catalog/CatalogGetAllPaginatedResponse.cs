using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Catalog
{
    [ExcludeFromCodeCoverage]
    public class CatalogGetAllPaginatedResponse : ModelResponseGetAll<CatalogGetAllRows> { }

    [ExcludeFromCodeCoverage]
    public class CatalogGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<CatalogGetAllPaginated> Rows { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class CatalogGetAllPaginated : CatalogResponse
    {
    }
}
