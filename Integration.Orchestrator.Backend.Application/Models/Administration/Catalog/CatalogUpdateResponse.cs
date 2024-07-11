namespace Integration.Orchestrator.Backend.Application.Models.Administration.Catalog
{
    public class CatalogUpdateResponse : ModelResponse<CatalogUpdate>
    {
    }
    public class CatalogUpdate : CatalogRequest
    {
        public Guid Id { get; set; }
    }
}
