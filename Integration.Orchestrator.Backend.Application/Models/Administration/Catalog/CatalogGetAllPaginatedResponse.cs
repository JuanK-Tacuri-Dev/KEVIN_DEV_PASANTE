namespace Integration.Orchestrator.Backend.Application.Models.Administration.Catalog
{
    public class CatalogGetAllPaginatedResponse : ModelResponseGetAll<CatalogGetAllRows> { }

    public class CatalogGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<CatalogGetAllPaginated> Rows { get; set; }
    }
    public class CatalogGetAllPaginated : CatalogResponse
    {
    }
}
