namespace Integration.Orchestrator.Backend.Application.Models.Administration.Catalog
{
    public class CatalogGetByIdResponse : ModelResponse<CatalogGetById>
    {
    }
    public class CatalogGetById : CatalogRequest
    {
        public Guid Id { get; set; }
    }
}
