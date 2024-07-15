namespace Integration.Orchestrator.Backend.Application.Models.Administration.Catalog
{
    public class CatalogGetByTypeResponse : ModelResponse<IEnumerable<CatalogGetByType>>
    {
    }
    public class CatalogGetByType : CatalogRequest
    {
        public Guid Id { get; set; }
    }
}
