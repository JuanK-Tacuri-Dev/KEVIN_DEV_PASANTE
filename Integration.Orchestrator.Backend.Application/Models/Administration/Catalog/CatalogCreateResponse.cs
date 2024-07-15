namespace Integration.Orchestrator.Backend.Application.Models.Administration.Catalog
{
    public class CatalogCreateResponse : ModelResponse<CatalogCreate>
    {
    }
    public class CatalogCreate : CatalogRequest
    {
        public Guid Id { get; set; }
    }
}
