namespace Integration.Orchestrator.Backend.Application.Models.Administration.Catalog
{
    public class CatalogGetByFatherResponse : ModelResponse<IEnumerable<CatalogGetByType>>
    {
    }
    public class CatalogGetByType : CatalogResponse
    {
        public Guid Id { get; set; }
    }
}
